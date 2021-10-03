using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;


namespace DataLayer
{
    public enum StateOfEntity
    {
        Added,
        Modified,
        Deleted,
    }

    public class ChangedEntity<E> where E : class
    {
        public E Entity { get; private set; }
        public StateOfEntity State { get; private set; }

        internal ChangedEntity(E entity, System.Data.Entity.EntityState state)
        {
            this.Entity = entity;

            switch (state)
            {
                case EntityState.Added: this.State = StateOfEntity.Added;
                    break;
                case EntityState.Deleted: this.State = StateOfEntity.Deleted;
                    break;                
                case EntityState.Modified: this.State = StateOfEntity.Modified;
                    break;                    
            }

        }
    }

    public abstract class GenericRepository<UoW, E, DefaultUoW> : IDisposable, IRepository<E, UoW>
        where UoW : IUnitOfWork
        where E : class
        where DefaultUoW : UoW
    {
        private readonly IContext context;
        private readonly IUnitOfWork uow;

        public static event EventHandler<IEnumerable<E>> Added;
        public static event EventHandler<IEnumerable<E>> Modified;
        public static event EventHandler<IEnumerable<E>> Deleted;
        public static event EventHandler<IEnumerable<ChangedEntity<E>>> Changed;
        public static event Action SaveCalled;

        private bool ContextDisposeOlacakMi = true;

        public GenericRepository(UoW uow)
        {
            this.uow = uow;
            this.context = (uow as GenericUnitOfWork).Context;            
            ContextDisposeOlacakMi = false;
        }

        public GenericRepository()
        {
            this.uow = Activator.CreateInstance<DefaultUoW>();
            this.context = (this.uow as GenericUnitOfWork).Context;
            ContextDisposeOlacakMi = true;
        }

        private DbSet<E> GetDbSet()
        {
            Type contextType = context.GetType();
            PropertyInfo[] properties = contextType.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(DbSet<E>))
                {
                    return property.GetValue(context, null) as DbSet<E>;
                    //return context.GetType().GetProperty(property.Name).GetValue(context, null) as DbSet<E>;
                }
            }
            return null;
        }

        public virtual IQueryable<E> All
        {
            get
            {
                return GetDbSet();
            }
        }

        public IQueryable<E> AllIncluding(params Expression<Func<E, object>>[] includeProperties)
        {
            IQueryable<E> query = GetDbSet();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        public IQueryable<E> AllIncluding(IQueryable<E> query, params Expression<Func<E, object>>[] includeProperties)
        {            
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }


        public E Find(params object[] keyValues)
        {
            return GetDbSet().Find(keyValues);
        }

        /// <summary>
        /// Eğer Id GuId ise ForceInsert değeri belirtilmelidir.
        /// </summary>
        public void InsertOrUpdate(E entity, bool ForceInsert = false)
        {
            var KeyIsDefault = true;
            foreach (var property in typeof(E).GetProperties())
            {
                Type tip = property.PropertyType;
                if (property.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute)).Count() > 0)
                {
                        if (tip == typeof(System.Guid))
                        {
                            var val = (System.Guid)property.GetValue(entity);
                            if (val == Guid.Empty)
                            {
                                property.SetValue(entity, Guid.NewGuid());
                                KeyIsDefault = true;
                            }
                            else
                            {
                                KeyIsDefault = false;
                            }
                        }
                        else
                            if (tip.IsValueType)
                            {
                                var def = Activator.CreateInstance(tip);
                                var val = property.GetValue(entity);

                                if (tip == typeof(System.Int32)) KeyIsDefault = ((int)val == 0) ? true : false;
                                else
                                    if (tip == typeof(System.Boolean)) KeyIsDefault = ((bool)val == false) ? true : false;
                                    else
                                        if (tip == typeof(System.Byte)) KeyIsDefault = ((byte)val == 0) ? true : false;
                                        else
                                            if (tip == typeof(System.Char)) KeyIsDefault = ((char)val == '\0') ? true : false;
                                            else
                                                if (val != def)
                                                {
                                                    KeyIsDefault = false;
                                                    break;
                                                }
                            }
                            else
                            {
                                if (property.GetValue(entity) != null)
                                {
                                    KeyIsDefault = false;
                                }
                                else
                                {
                                    KeyIsDefault = true;
                                }
                            }
                }
            }

            if (KeyIsDefault || ForceInsert)
            {
                context.Entry(entity).State = System.Data.Entity.EntityState.Added;
            }
            else
            {
                context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            }       
        }

        public virtual void Delete(params object[] keyValues)
        {
            var entitiy = Find(keyValues);
            if (entitiy != null)
                GetDbSet().Remove(entitiy);
        }

        public virtual int Save()
        {
            var insertedList = context.ObjectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added);
            var modifiedList = context.ObjectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Modified);
            var deletedList = context.ObjectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Deleted);
            var changedList = new List<ChangedEntity<E>>();

            if (deletedList != null)
            {
                if (deletedList.Count() > 0)
                {
                    deletedList.Select(e => e).ToList().ForEach(e => changedList.Add(new ChangedEntity<E>(e.Entity as E, e.State)));

                    var deldeleted = Deleted as EventHandler<IEnumerable<E>>;
                    if (deldeleted != null)
                    {
                        deldeleted("", deletedList.Select(e => e.Entity as E).ToList());
                    }
                }
            }

            int returnValue = context.SaveChanges();

            var delSaveCalled = SaveCalled as Action;
            if (delSaveCalled != null)
            {
                delSaveCalled();
            }

            if (insertedList != null)
            {
                if (insertedList.Count() > 0)
                {
                    insertedList.Select(e => e).ToList().ForEach(e => changedList.Add(new ChangedEntity<E>(e.Entity as E, e.State)));

                    var delInserted = Added as EventHandler<IEnumerable<E>>;
                    if (delInserted != null)
                    {
                        delInserted("", insertedList.Select(e => e.Entity as E).ToList());
                    }
                }
            }

            if (modifiedList != null)
            {
                if (modifiedList.Count() > 0)
                {
                    modifiedList.Select(e => e).ToList().ForEach(e => changedList.Add(new ChangedEntity<E>(e.Entity as E, e.State)));

                    var delmodified = Modified as EventHandler<IEnumerable<E>>;
                    if (delmodified != null)
                    {
                        delmodified("", modifiedList.Select(e => e.Entity as E).ToList());
                    }
                }
            }           


            if (changedList != null)
            {
                if (changedList.Count > 0)
                {
                    var delchanged = Changed as EventHandler<IEnumerable<ChangedEntity<E>>>;
                    if (delchanged != null)
                    {
                        delchanged("", changedList);
                    }
                }
            }

            return returnValue;
        }

        public IEnumerable<T> SqlQuery<T>(string Sql, params object[] parametreler)
        {
            List<System.Data.SqlClient.SqlParameter> parameters = new List<System.Data.SqlClient.SqlParameter>();

            for (int i = 0; i < parametreler.Length; i++)
            {
                parameters.Add(new System.Data.SqlClient.SqlParameter("param" + (i + 1).ToString(), parametreler[i]));
            }

            IEnumerable<T> r;

            using (var ctx = context as DbContext)
            {
                r = ctx.Database.SqlQuery<T>(Sql, parameters.ToArray()).ToList();
            }

            return r;
        }

        public IEnumerable<E> SqlQuery(string Sql, params object[] parametreler)
        {
            List<System.Data.SqlClient.SqlParameter> parameters = new List<System.Data.SqlClient.SqlParameter>();

            for (int i = 0; i < parametreler.Length; i++)
            {
                parameters.Add(new System.Data.SqlClient.SqlParameter("param" + (i  + 1).ToString(), parametreler[i]));
            }

            IEnumerable<E> r;

            using (var ctx = context as DbContext)
            {
                r = ctx.Database.SqlQuery<E>(Sql, parameters.ToArray()).ToList();
            }

            return r;
        }

        public IQueryable<E> FreeText(string containsSearchText)
        {            

                var ctx = context as DbContext;
                return All.FreeTextContains(ctx, containsSearchText);

        }

        public IEnumerable<E> FreeText(string searchText,Expression<Func<E, object>> selectColumn, params Expression<Func<E, object>>[] searchColumns)
        {
            string tableName = typeof(E).Name;

            string primaryKeyColumnName = "Id";

            var properties = typeof(E).GetProperties();
            foreach (var property in properties)
            {
                Type tip = property.PropertyType;
                if (property.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute)).Count() > 0)
                {
                    primaryKeyColumnName = property.Name;
                    break;
                }
            }

            string selectColumnNameText = "D." + selectColumn.Body.ToString().Substring(selectColumn.Body.ToString().LastIndexOf(".") + 1).Replace("(","").Replace(")","");

            string searchColumnNameText = "";
            List<string> searchColumnNames = new List<string>();
            foreach (var searchColumn in searchColumns)
            {
                searchColumnNames.Add(searchColumn.Body.ToString().Substring(searchColumn.Body.ToString().LastIndexOf(".") + 1));
            }
            searchColumnNameText = String.Join(",", searchColumnNames);            

            string sqlQuery = "SELECT @param1 FROM FREETEXTTABLE(@param2, @param3, @param4, 20) AS CT  INNER JOIN @param2 AS D ON CT.[KEY] = @param5  ORDER BY CT.[RANK] DESC;";
            //                "SELECT @selectColumnNameText FROM FREETEXTTABLE(@tableName,@searchColumnNameText,N'@searchText',20) AS CT  INNER JOIN @tableName AS D ON CT.[KEY] = D.@primaryKeyColumnName  ORDER BY CT.[RANK] DESC;", primaryKeyColumnName, tableName, commandType, searchColumnNameText, searchText, selectColumnNameText);

            return SqlQuery(sqlQuery, selectColumnNameText, tableName, searchColumnNameText, searchText, primaryKeyColumnName);

            //List<string> searchColumnNames = new List<string>();

            //foreach (var column in searchColumns)
            //{
            //    string expressionBody = column.Body.ToString();
            //    searchColumnNames.Add(expressionBody.Substring(expressionBody.IndexOf('.') + 1));
            //}

            //string[] selectColumnName = new string[1];
            //string letters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890_";
            //string selectColumnExpressionBody = selectColumn.Body.ToString();            
            //selectColumnName[0] = selectColumnExpressionBody.Substring(selectColumnExpressionBody.LastIndexOf('.') + 1);
            //for (int i = 0; i < selectColumnName[0].Length; i++)
            //{
            //    int j = letters.IndexOf(selectColumnName[0][i]);
            //    if (j < 0)
            //    {
            //        selectColumnName[0] = selectColumnName[0].Substring(0, i);
            //        break;
            //    }
            //}

            //var ctx = context as DbContext;
            //return All.FreeTextSearch(ctx, containsSearchText, searchColumnNames.ToArray(), selectColumnName);
        }

        public void Dispose()
        {
            if (ContextDisposeOlacakMi)
            {
                context.Dispose();
                uow.Dispose();
            }            
        }
    }
}