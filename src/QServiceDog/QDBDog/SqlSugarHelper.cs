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

        public static string Exec(string script, string serverName, string dbname = "master")
        {
            string file = System.IO.Path.GetTempFileName();
            System.IO.File.WriteAllText(file, script, Encoding.UTF8);
            try
            {
                return ExecFile(file, serverName, dbname);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                try
                {
                    System.IO.File.Delete(file);
                }
                catch
                {

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
        public static string ExecFile(string file, string serverName, string dbname = "master")
        {
            //查找所有可用的sqlcmd,从高版本往低版本查 150-90，找不到就抛错
            if (!System.IO.File.Exists(file))
                return "";
            string sqlcmd = "";
            for (int i = 220; i > 80; i -= 10)
            {
                Microsoft.Win32.RegistryKey reg = Microsoft.Win32.Registry.LocalMachine.OpenSubKey($"SOFTWARE\\Microsoft\\Microsoft SQL Server\\{i}\\Tools\\ClientSetup");
                if (reg == null)
                    continue;
                sqlcmd = System.IO.Path.Combine(reg.GetValue("Path", "").ToString(), "sqlcmd.exe");
                if (System.IO.File.Exists(sqlcmd))
                    break;//找到第一个可用的
            }
            if (string.IsNullOrEmpty(sqlcmd))
            {
                throw new Exception("本机没有安装SQLServer，无法执行脚本");
            }


            System.Diagnostics.Process pro = new System.Diagnostics.Process();
            pro.StartInfo.FileName = sqlcmd;
            pro.StartInfo.UseShellExecute = false;
            pro.StartInfo.RedirectStandardInput = true;
            pro.StartInfo.RedirectStandardOutput = true;
            pro.StartInfo.RedirectStandardError = true;
            pro.StartInfo.CreateNoWindow = true;

            ////解析连接信息
            //System.Collections.Hashtable properties = new System.Collections.Hashtable();
            //char[] ch = new char[] { '=' };
            //foreach (String item in ConnectionString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            //{

            //    if (item.Split(ch).Length != 2)
            //        continue;
            //    properties.Add(item.Split(ch)[0].Trim().ToUpper(), item.Split(ch)[1]);
            //}
            pro.StartInfo.Arguments = $"-S{serverName} -d{dbname} -E -i\"{file}\"";
            pro.Start();


            pro.WaitForExit();
            string result = pro.StandardOutput.ReadToEnd();
            return result;
        }


    }
}
