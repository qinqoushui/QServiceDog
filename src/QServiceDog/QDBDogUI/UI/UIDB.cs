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
            execSql(config, Resources.AutoBakCert, (sender as Button).Text);
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
            execSql(config, Resources.DropAutoBakCert, (sender as Button).Text);
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
    }
}
