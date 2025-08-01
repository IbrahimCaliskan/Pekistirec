﻿using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace DataLayer
{
    public class SoftDeleteQueryVisitor : DefaultExpressionVisitor
    {
        public override DbExpression Visit(DbScanExpression expression)
        {
            var column = SoftDeleteAttribute.GetSoftDeleteColumnName(expression.Target.ElementType);
            if (column != null)
            {
                // Just because the entity has the soft delete annotation doesn't mean that 
                // this particular table has the column. This occurs in situation like TPT
                // inheritance mapping and entity splitting where one type maps to multiple 
                // tables.
                // We only apply the filter if the column is actually present in this table.
                // If not, then the query is going to be joining to the table that does have
                // the column anyway, so the filter will still be applied.
                var table = (EntityType)expression.Target.ElementType;
                if (table.Properties.Any(p => p.Name == column))
                {
                    var binding = DbExpressionBuilder.Bind(expression);
                    return DbExpressionBuilder.Filter(
                        binding,
                        DbExpressionBuilder.NotEqual(
                            DbExpressionBuilder.Property(
                                DbExpressionBuilder.Variable(binding.VariableType, binding.VariableName),
                                column),
                            DbExpression.FromBoolean(true)));
                }
            }

            return base.Visit(expression);
        }
    }
}
