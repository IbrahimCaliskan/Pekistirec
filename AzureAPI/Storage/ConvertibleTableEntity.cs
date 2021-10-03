using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AzureAPI.Storage
{
    internal class ConvertibleTableEntity<kendiT, digerT> : TableEntity
    {
        internal digerT Convert()
        {
            Type mTip = typeof(kendiT);
            kendiT model = (kendiT)((object)this);
            PropertyInfo[] mProperties = mTip.GetProperties();

            Type vmTip = typeof(digerT);
            digerT digermodel = (digerT)Activator.CreateInstance(vmTip);
            PropertyInfo[] vmProperties = vmTip.GetProperties();


            foreach (var item in vmProperties)
            {
                PropertyInfo Mp = mProperties.Where(p => p.Name == item.Name).Select(p => p).SingleOrDefault();
                if (Mp != null)
                    item.SetValue(digermodel, Mp.GetValue(model, null));
            }

            return digermodel;
        }

        internal void Absorb(digerT Model)
        {
            Type kendiTip = typeof(kendiT);
            PropertyInfo[] kendiProperties = kendiTip.GetProperties();

            Type digerTip = typeof(digerT);
            PropertyInfo[] digerProperties = digerTip.GetProperties();

            int i = 1;

            foreach (var item in digerProperties)
            {
                if (i == 1)
                {
                    this.PartitionKey = item.GetValue(Model, null).ToString();
                }
                if (i == 2)
                {
                    this.RowKey = item.GetValue(Model, null).ToString();
                }
                
                if (i > 2)
                {
                    if (kendiProperties.Any(p => ((p.Name == item.Name) && (p.PropertyType.FullName == item.PropertyType.FullName))))
                    {
                        PropertyInfo property = kendiProperties.Where(p => p.Name == item.Name).Select(p => p).SingleOrDefault();
                        if (property != null)
                        {
                            property.SetValue((kendiT)((object)this), item.GetValue(Model, null));
                        }
                    }
                }

                i++;
            }
        }

        //public kendiT Cast()
        //{
        //    Type mTip = typeof(kendiT);
        //    kendiT model = (kendiT)Activator.CreateInstance(mTip);
        //    PropertyInfo[] mProperties = mTip.GetProperties();

        //    kendiT fakeModel = (kendiT)Activator.CreateInstance(mTip);
        //    fakeModel = (kendiT)((object)this);

        //    foreach (var property in mProperties)
        //    {
        //        try
        //        {
        //            property.SetValue(model, property.GetValue(fakeModel, null), null);
        //        }
        //        catch (System.Reflection.TargetInvocationException)
        //        {
        //            // Alt tablonun değeri yok                    
        //        }
        //    }

        //    return model;
        //}
    }
}
