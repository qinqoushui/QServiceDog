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

        /// <summary>
        /// 使用sqlcmd执行脚本
        /// </summary>
        /// <param name="file"></param>
        /// <param name="deleteWhenSuccess"></param>
        /// <param name="dbname"></param>
        /// <returns></returns>
        public static string ExecFile(string file, bool deleteWhenSuccess, string dbname)
        {
            //查找所有可用的sqlcmd,从高版本往低版本查 150-90，找不到就抛错
            if (!System.IO.File.Exists(file))
                return "";
            string sqlcmd = "";
            try
            {
                Microsoft.Win32.RegistryKey reg = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(string.Format(@"SOFTWARE\Microsoft\Microsoft SQL Server\{0}\Tools\ClientSetup", Properties.Settings.Default.sqlver));
                if (reg == null)
                {
                    throw new Exception("\r\n本机没有安装SQL2005，请与管理人员联系以进行数据库升级\r\n");
                }
                sqlcmd = System.IO.Path.Combine(reg.GetValue("Path").ToString(), "sqlcmd.exe");
                if (!System.IO.File.Exists(sqlcmd))
                    return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            System.Diagnostics.Process pro = new System.Diagnostics.Process();
            pro.StartInfo.FileName = sqlcmd;
            pro.StartInfo.UseShellExecute = false;
            pro.StartInfo.RedirectStandardInput = true;
            pro.StartInfo.RedirectStandardOutput = true;
            pro.StartInfo.RedirectStandardError = true;
            pro.StartInfo.CreateNoWindow = true;

            pro.StartInfo.Arguments = string.Format("-S{0} -d{1} -U{3} -P{4} -i\"{2}\"",
                ".\\sqlexpress", dbname, file, "sa", "Sa1234567");
            pro.Start();


            pro.WaitForExit();
            string result = pro.StandardOutput.ReadToEnd();

            if (deleteWhenSuccess && System.IO.File.Exists(file))
                System.IO.File.Delete(file);

            return result;
        }
    }
}
