using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDBDog.Share
{
    public class BackupHelper
    {
        static Q.Helper.ILogHelper logger = Q.Helper.LogHelper.Default.Get(nameof(BackupHelper));
        static BackupHelper _ = new BackupHelper();
        public static BackupHelper Instance { get; } = _;

        public (string flag, string err) Backupdb(Config config, string template, out string subPath, Action<string> showResult = null)
        {
            subPath = string.Empty;
            List<string> selected = new List<string>();
            if (string.IsNullOrEmpty(config.DBNames))
            {
                return ("skip", "没有需要备份的数据库");
            }
            List<string> dbList = new List<string>();
            string error = string.Empty;
            new SqlSugarHelper($"server={config.DBServer};Initial Catalog=master;Integrated Security=True;Connection Timeout=5;").DoOne(client =>
            {
                try
                {
                    var dt = client.Ado.GetDataTable(" SELECT name FROM   sys.databases WHERE    state = 0 order by name");
                    if (dt.Rows.Count > 0)
                    {
                        dbList = dt.Select().Select(row => row[0].ToString()).ToList();
                    }
                }
                catch (Exception ex)
                {
                    error = ex.ToString();
                }
            });
            if (!string.IsNullOrEmpty(error))
                return ("error", error);
            if (dbList.Count == 0)
            {
                return ("skip", "没有需要备份的数据库");
            }

            selected = config.DBNames.Split(',').ToList();
            List<string> needBackup = dbList.Where(row => selected.Exists(r => r.Equals(row, StringComparison.OrdinalIgnoreCase))).ToList();
            if (needBackup.Count == 0)
            {
                return ("skip", $"需要备份的数据库{config.DBNames}在当前服务器{config.DBServer}中不存在");
            }
            StringBuilder sb = new StringBuilder();
            string time = DateTime.Now.ToString("yyyy_MM_dd_HHmmss_ff");
            subPath = Path.Combine(config.LocalPath, time);
            if (!Directory.Exists(subPath))
                Directory.CreateDirectory(subPath);
            File.WriteAllText(Path.Combine(subPath, "备份说明.md"), $"1. 节点名称:{config.Name} \r\n1. 时间:{time} \r\n1. 期望备份的数据库:{config.DBNames} \r\n1. 实际备份的数据库:{string.Join(",", needBackup.ToArray())}");
            foreach (var db in needBackup)
            {
                sb.Append(template.Replace("@dbname@", db).Replace("@path@", Path.Combine(subPath, db)));
                sb.AppendLine();
            }
            var result = SQLServerHelper.Exec(sb.ToString(), config.DBServer);
            File.AppendAllText(Path.Combine(subPath, "备份说明.md"), $"\r\n\r\n-\r\n{result}", Encoding.UTF8);
            showResult?.Invoke(result);
            return ("succ", result);
        }

        public (string flag, string err) ClearBackupFiles(Config config)
        {
            if (!Directory.Exists(config.LocalPath))
                return ("skip", $"{config.LocalPath}不存在");
            int c = 0, t = 0, e = 0;
            switch (config.ExpireType.ToUpper())
            {
                case "NUM":
                    //一级子目录中备份说明.md最后修改的三个子目录保留
                    List<FileInfo> tmpInfo = new List<FileInfo>();
                    foreach (var sub in new DirectoryInfo(config.LocalPath).GetDirectories())
                    {
                        var aa = sub.GetFiles("备份说明.md");
                        if (aa.Length == 0)
                            continue;
                        tmpInfo.Add(aa.First());
                    }
                    tmpInfo = tmpInfo.OrderByDescending(r => r.LastWriteTime).ToList();
                    if (tmpInfo.Count > config.ExpireValue)
                    {
                        tmpInfo.Skip(config.ExpireValue).Take(tmpInfo.Count - config.ExpireValue).ToList().ForEach(sub =>
                        {
                            //删除所有文件
                            var files = sub.Directory.GetFiles("*", SearchOption.AllDirectories);
                            t += files.Length;
                            foreach (var f in files)
                            {
                                try
                                {
                                    f.Delete();
                                    c++;
                                }
                                catch
                                {
                                    e++; //无所谓，总有删除的一天
                                }

                            }
                        });
                    }
                    break;
                case "DAY":
                default:
                    //删除指定日期以前的文件
                    DateTime last = DateTime.Today.AddDays(0 - config.ExpireValue);
                    //一级子目录中必须有 备份说明.md
                    foreach (var sub in new DirectoryInfo(config.LocalPath).GetDirectories())
                    {
                        if (sub.GetFiles("备份说明.md").Length == 0)
                            continue;
                        var files = sub.GetFiles("*", SearchOption.AllDirectories);
                        t += files.Length;
                        foreach (var f in files)
                        {
                            if (f.LastWriteTime < last)
                            {
                                try
                                {
                                    f.Delete();
                                    c++;
                                }
                                catch
                                {
                                    e++; //无所谓，总有删除的一天
                                }
                            }
                        }
                    }
                    break;
            }
            //删除空目录,不递归，总有删除的一天
            new DirectoryInfo(config.LocalPath).GetDirectories("*", SearchOption.AllDirectories).ToList().ForEach(r =>
            {
                if (r.GetFiles().Length == 0 && r.GetDirectories().Length == 0)
                    try { r.Delete(); } catch { }
            });
            if (e > 0)
                return ("succ", $"清理结果:{c}/{t},失败{e}");
            else
                return ("succ", $"清理结果:{c}/{t}");

        }

        public (string flag, string err) FtpFiles(Config config, string ftpExe, string ftpScript)
        {
            //运行脚本
            if (System.Diagnostics.Process.GetProcessesByName("FreeFileSync").Length > 0)
            {
                //TODO:判断进程使用的时间
                return ("skip", "已在运行中!!!");
            }
            if (!File.Exists(ftpScript))
            {
                return ("error", "脚本尚未生成");
            }
            //#if DEBUG
            //            //临时显示界面 <ProgressDialog Minimized="true" AutoClose="true"/>
            //            string testFile = ftpScript + "_test.ffs_batch";
            //            System.IO.File.WriteAllText(testFile, System.IO.File.ReadAllText(ftpScript, Encoding.UTF8).Replace(@"<ProgressDialog Minimized=""true"" AutoClose=""true""/>", @"<ProgressDialog Minimized=""false"" AutoClose=""false""/>"), Encoding.UTF8);
            //            var p = System.Diagnostics.Process.Start(ftpExe, testFile);
            //#else
            //            var p = System.Diagnostics.Process.Start(ftpExe, ftpScript);
            //#endif
            var p = System.Diagnostics.Process.Start(ftpExe, ftpScript);

            logger.Info($"FTP脚本开始执行于{DateTime.Now.ToString("G")}");
            p.WaitForExit();
            p.Dispose();
            return ("succ", "");
        }

    }

    public class SQLServerHelper
    {
        static Q.Helper.ILogHelper logger = Q.Helper.LogHelper.Default.Get(nameof(SQLServerHelper));
        public static string Exec(string script, string serverName, string dbname = "master")
        {
            logger.Info("执行脚本：" + script);
            string file = System.IO.Path.GetTempFileName();
            System.IO.File.WriteAllText(file, script, Encoding.UTF8);
            try
            {
                var result = ExecFile(file, serverName, dbname);
                logger.Info("执行结果：" + result);
                return result;
            }
            catch (Exception ex)
            {
                logger.Info("执行结果：" + ex.ToString());
                return ex.ToString();
            }
            finally
            {
                try
                {
                    System.IO.File.Delete(file);
                    logger.Info($"删除文件：{file}");
                }
                catch
                {

                }
            }
        }
        public static string ExecFile(string file, string serverName, string dbname = "master")
        {
            logger.Info("执行脚本：" + File.ReadAllText(file, Encoding.UTF8));
            try
            {
                var result = execFile(file, serverName, dbname);
                logger.Info("执行结果：" + result);
                return result;
            }
            catch (Exception ex)
            {
                logger.Info("执行结果：" + ex.ToString());
                return ex.ToString();
            }
        }
        /// <summary>
        /// 使用sqlcmd执行脚本
        /// </summary>
        /// <param name="file"></param>
        /// <param name="deleteWhenSuccess"></param>
        /// <param name="dbname"></param>
        /// <returns></returns>
        static string execFile(string file, string serverName, string dbname = "master")
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
