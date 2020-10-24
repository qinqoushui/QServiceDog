using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDBDog
{
    public class SqlSugarHelper
    {
        string ConnectionString { get; set; }
        public SqlSugarHelper(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public SqlSugarClient Client
        {
            get
            {
                SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = ConnectionString,
                    DbType = DbType.SqlServer,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute
                });

#if DEBUG
                //Print sql
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    Console.WriteLine(sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                    Console.WriteLine();
                };
#endif
                return db;
            }
        }

        public T DoOne<T>(Func<SqlSugarClient, T> a)
        {
            using (var db = Client)
            {
                try
                {
                    return a(db);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void DoOne(Action<SqlSugarClient> a)
        {
            using (var db = Client)
            {
                try
                {
                    a(db);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

    }
}
