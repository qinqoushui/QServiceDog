using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QDBDogUI.Properties;
using QDBDog;
using System.Configuration;
using System.IO;

namespace QDBDogUI.UI
{
    public partial class UIDB : UIBase
    {
        public UIDB()
        {
            InitializeComponent();
            theTableLayoutPanel = tableLayoutPanel1;
            btnDefault.Click += (s, e) => loadDefault();
            btnSave.Click += (s, e) => save();
            btnLoad.Click += (s, e) => loadConfigFromFile();

        }

        protected override void collectSub(Dictionary<string, string> data)
        {
            base.collectSub(data);
            List<string> ss = new List<string>();
            foreach (var a in checkedListBox1.CheckedItems)
                ss.Add(a.ToString());
            if (ss.Count == 0)
                data["DBNames"] = string.Empty;
            else
                data["DBNames"] = string.Join(",", ss.ToArray());
        }


        protected override void render(Config config)
        {
            base.render(config);
            checkedListBox1.Items.Clear();
            List<string> selected = new List<string>();
            if (!string.IsNullOrEmpty(config.DBNames))
            {
                selected = config.DBNames.Split(',').ToList();
            }
            foreach (var row in selected)
            {
                checkedListBox1.Items.Add(row, true);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lockButton(5, sender as Button);

            new QDBDog.SqlSugarHelper($"server={tbServer.Text};Initial Catalog=master;Integrated Security=True;Connection Timeout=5;").DoOne(client =>
            {
                try
                {
                    var dt = client.Ado.GetDataTable(" SELECT name FROM   sys.databases WHERE    state = 0 order by name");
                    checkedListBox1.Items.Clear();
                    //    checkedListBox1.Items.AddRange(dt.Select().Select(r => r[0].ToString()).ToArray());
                    //标记已选择的
                    var old = loadConfig();
                    List<string> selected = new List<string>();
                    if (!string.IsNullOrEmpty(old.DBNames))
                    {
                        selected = old.DBNames.Split(',').ToList();
                    }
                    foreach (DataRow row in dt.Rows)
                    {
                        checkedListBox1.Items.Add(row[0].ToString(), selected.Exists(r => r.Equals(row[0].ToString(), StringComparison.OrdinalIgnoreCase)));
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var config = loadConfig();
            if (MessageBox.Show($"将为当前指定的数据库服务器{config.DBServer}启用备份加密密钥和证书，启用证书后，备份文件将无法在未使用特定证书的服务器上还原，是否继续？", "数据库配置", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
            //检查指定目录下有无证书
            string certFile = System.IO.Path.Combine(Application.StartupPath, "sql\\AutoBakCert.cer");
            string pkFile = System.IO.Path.Combine(Application.StartupPath, "sql\\AutoBakCert.pkey");
            if (!System.IO.File.Exists(certFile) || !System.IO.File.Exists(pkFile))
            {
                MessageBox.Show("加密证书不存在！操作失败");
                return;
            }
            execSql(config, string.Format(Resources.AutoBakCert, System.IO.Path.Combine(Application.StartupPath, "sql")), (sender as Button).Text);
            //new QDBDog.SqlSugarHelper($"server={tbServer.Text};Initial Catalog=master;Integrated Security=True;Connection Timeout=5;").DoOne(client =>
            //{
            //    try
            //    {
            //        client.Ado.ExecuteCommand(System.Text.RegularExpressions.Regex.Replace(sqlFile, @"^\s*go\s*\n", "\n", System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase));
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //});
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var config = loadConfig();
            if (MessageBox.Show($"将为当前指定的数据库服务器{config.DBServer}上移除备份加密密钥和证书，移除后，备份文件可以在任意服务器上还原，这可能导致数据泄露，是否继续？", "数据库配置", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
            execSql(config, string.Format(Resources.DropAutoBakCert, System.IO.Path.Combine(Application.StartupPath, "sql")), (sender as Button).Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //从资源中释放证书
            if (MessageBox.Show($"从程序资源文件中释放默认证书，当前证书文件将被覆盖，是否继续？", "数据库配置", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
            //检查指定目录下有无证书
            string certFile = System.IO.Path.Combine(Application.StartupPath, "sql\\AutoBakCert.cer");
            string pkFile = System.IO.Path.Combine(Application.StartupPath, "sql\\AutoBakCert.pkey");
            System.IO.File.WriteAllBytes(certFile, Resources.AutoBakCert1);
            System.IO.File.WriteAllBytes(pkFile, Resources.AutoBakCert2);
            MessageBox.Show("OK");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var config = loadConfig();
            if (System.IO.Directory.Exists(config.LocalPath))
                System.Diagnostics.Process.Start("explorer.exe", config.LocalPath);
            else
                MessageBox.Show($"{config.LocalPath}不存在");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //生成备份脚本并执行
            lockButton(30, sender as Button);
            new QDBDog.SqlSugarHelper($"server={tbServer.Text};Initial Catalog=master;Integrated Security=True;Connection Timeout=5;").DoOne(client =>
            {
                try
                {
                    var dt = client.Ado.GetDataTable(" SELECT name FROM   sys.databases WHERE    state = 0 order by name");
                    backupdb(Resources.BackupDB, dt.Select().Select(row => row[0].ToString()).ToList(), true, out var subPath);
                    if (!string.IsNullOrEmpty(subPath))
                        System.Diagnostics.Process.Start("explorer.exe", subPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }

        string backupdb(string script, List<string> dbList, bool showResult, out string subPath)
        {
            subPath = string.Empty;
            var old = loadConfig();
            List<string> selected = new List<string>();
            if (string.IsNullOrEmpty(old.DBNames) || dbList.Count == 0)
            {
                MessageBox.Show("没有需要备份的数据库");
                return string.Empty;
            }
            selected = old.DBNames.Split(',').ToList();
            List<string> needBackup = dbList.Where(row => selected.Exists(r => r.Equals(row, StringComparison.OrdinalIgnoreCase))).ToList();
            if (needBackup.Count == 0)
            {
                MessageBox.Show("没有需要备份的数据库");
                return string.Empty;
            }
            var config = loadConfig(false);
            StringBuilder sb = new StringBuilder();
            string time = DateTime.Now.ToString("yyyy_MM_dd_HHmmss_ff");
            subPath = System.IO.Path.Combine(config.LocalPath, time);
            if (!System.IO.Directory.Exists(subPath))
                System.IO.Directory.CreateDirectory(subPath);
            File.WriteAllText(Path.Combine(subPath, "备份说明.md"), $"1. 节点名称:{config.Name} \r\n1. 时间:{time} \r\n1. 期望备份的数据库:{config.DBNames} \r\n1. 实际备份的数据库:{string.Join(",", needBackup.ToArray())}");
            foreach (var db in needBackup)
            {
                sb.Append(script.Replace("@dbname@", db).Replace("@path@", System.IO.Path.Combine(subPath, db)));
                sb.AppendLine();
            }
            logger.Debug(sb.ToString());
            return execSql(config, sb.ToString(), "备份数据库", showResult);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //按规则清理备份
            try
            {
                var config = loadConfig(false);
                string result = clearBackupFiles(config.LocalPath, config.ExpireType, config.ExpireValue);
                MessageBox.Show(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        string clearBackupFiles(string path, string type, int num)
        {
            if (!Directory.Exists(path))
                return $"{path}不存在，跳过";
            int c = 0, t = 0, e = 0;
            switch (type.ToUpper())
            {
                case "NUM":
                    //一级子目录中备份说明.md最后修改的三个子目录保留
                    List<FileInfo> tmpInfo = new List<FileInfo>();
                    foreach (var sub in new DirectoryInfo(path).GetDirectories())
                    {
                        var aa = sub.GetFiles("备份说明.md");
                        if (aa.Length == 0)
                            continue;
                        tmpInfo.Add(aa.First());
                    }
                    tmpInfo = tmpInfo.OrderByDescending(r => r.LastWriteTime).ToList();
                    if (tmpInfo.Count > num)
                    {
                        tmpInfo.Skip(num).Take(tmpInfo.Count - num).ToList().ForEach(sub =>
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
                    DateTime last = DateTime.Today.AddDays(0 - num);
                    //一级子目录中必须有 备份说明.md
                    foreach (var sub in new DirectoryInfo(path).GetDirectories())
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
            new DirectoryInfo(path).GetDirectories("*", SearchOption.AllDirectories).ToList().ForEach(r =>
            {
                if (r.GetFiles().Length == 0 && r.GetDirectories().Length == 0)
                    try { r.Delete(); } catch { }
            });
            return $"清理结果:{c}/{t},失败{e}";
        }
    }
}
