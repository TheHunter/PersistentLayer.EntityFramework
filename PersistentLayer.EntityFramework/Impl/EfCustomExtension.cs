using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Text;
using PersistentLayer.EntityFramework.Future;
using PersistentLayer.EntityFramework.Reflection;

namespace PersistentLayer.EntityFramework.Impl
{
    internal static class EfCustomExtension
    {
        internal static DbCommand CreateFutureCommand(this ObjectContext context, IEnumerable<IFutureQuery> futureQueries)
        {
            DbConnection dbConnection = context.Connection;
            var entityConnection = dbConnection as EntityConnection;

            // by-pass entity connection, doesn't support multiple results.
            var command = entityConnection == null
                              ? dbConnection.CreateCommand()
                              : entityConnection.StoreConnection.CreateCommand();

            dynamic entityConnectionProxy = new DynamicProxy(entityConnection);

            if (entityConnection != null && entityConnectionProxy.CurrentTransaction != null)
            {
                // .StoreTransaction is internal... verify the access..
                command.Transaction = entityConnectionProxy.CurrentTransaction.StoreTransaction;
            }

            var futureSql = new StringBuilder();
            int queryCount = 0;

            foreach (IFutureQuery futureQuery in futureQueries)
            {
                var plan = futureQuery.GetPlan(context);
                string sql = plan.CommandText;

                // clean up params
                foreach (var parameter in plan.Parameters)
                {
                    string orginal = parameter.Name;
                    string updated = string.Format("f{0}_{1}", queryCount, orginal);

                    sql = sql.Replace("@" + orginal, "@" + updated);

                    var dbParameter = command.CreateParameter();
                    dbParameter.ParameterName = updated;
                    if (parameter.Value == null)
                    {
                        dbParameter.Value = DBNull.Value;
                    }
                    else
                    {
                        dbParameter.Value = parameter.Value;
                    }

                    command.Parameters.Add(dbParameter);
                }

                // add sql
                if (futureSql.Length > 0)
                    futureSql.AppendLine();

                futureSql.Append("-- Query #");
                futureSql.Append(queryCount + 1);
                futureSql.AppendLine();
                futureSql.AppendLine();

                futureSql.Append(sql.Trim());
                futureSql.AppendLine(";");

                queryCount++;
            } // foreach query

            command.CommandText = futureSql.ToString();
            if (context.CommandTimeout.HasValue)
                command.CommandTimeout = context.CommandTimeout.Value;

            return command;
        }
    }
}
