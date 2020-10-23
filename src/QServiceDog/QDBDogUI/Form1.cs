using QDBDogUI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QDBDogUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Shown += Form1_Shown;
        }
        string defaultFileName = System.IO.Path.Combine(Application.StartupPath, "config\\appsettings.json");

        private void Form1_Shown(object sender, EventArgs e)
        {
            //加载当前值或默认值
            if (System.IO.File.Exists(defaultFileName))
            {
                string s = System.IO.File.ReadAllText(defaultFileName, Encoding.UTF8);
                var config = s.De<QDBDog.Config>();
                if (config != null)
                {
                    render(config);
                }
            }
            else
                button7.PerformClick();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("恢复默认配置？当前的修改将全部丢失", "备份配置", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
            render(new QDBDog.Config());
        }

        void render(QDBDog.Config config)
        {
            var data = config.ToDict();
            foreach (var control in tableLayoutPanel1.Controls)
            {
                var ac = control as TextBox;
                if (ac != null && ac.Tag != null)
                {
                    var a = data.FirstOrDefault(r => r.Key.Equals(ac.Tag.ToString(), StringComparison.OrdinalIgnoreCase));
                    ac.Text = a.Value;
                }
            }
            foreach (var control in panel1.Controls)
            {
                var ac = control as RadioButton;
                if (ac != null && ac.Tag != null)
                {
                    var a = data.FirstOrDefault(r => r.Key.Equals(panel1.Tag.ToString(), StringComparison.OrdinalIgnoreCase));
                    ac.Checked = a.Value == ac.Tag.ToString();
                }
            }
            tbName.Text = config.Name;
            numExpire.Value = config.ExpireValue;
        }
        bool collect(out QDBDog.Config config)
        {
            var data = new Dictionary<string, string>();
            foreach (var control in tableLayoutPanel1.Controls)
            {
                var ac = control as TextBox;
                if (ac != null && ac.Tag != null)
                {
                    data[ac.Tag.ToString()] = ac.Text;
                }
            }
            foreach (var control in panel1.Controls)
            {
                var ac = control as RadioButton;
                if (ac != null && ac.Tag != null)
                {
                    if (ac.Checked)
                        data[ac.Tag.ToString()] = ac.Tag.ToString();
                }
            }
            data["Name"] = tbName.Text;
            data["ExpireValue"] = numExpire.Value.ToString("0");
            config = data.ToArray().FromNameValuePairs<QDBDog.Config>();
            return true;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("保存当前配置？", "备份配置", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
            if (collect(out var c))
            {
                string fileName = defaultFileName;
                if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(fileName)))
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fileName));
                if (System.IO.File.Exists(fileName))
                    System.IO.File.Copy(fileName, $"{fileName}.{DateTime.Now.Minute % 10 }.json.bak", true);
                System.IO.File.WriteAllText(fileName, c.Serialize(), Encoding.UTF8);
                MessageBox.Show("OK");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //从指定文件加载 
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "配置文件|*.json;*.json.bak";
                dlg.FilterIndex = 0;
                dlg.CheckFileExists = true;
                dlg.Multiselect = false;
                dlg.Title = "选择配置文件";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string s = System.IO.File.ReadAllText(dlg.FileName, Encoding.UTF8);
                    var config = s.De<QDBDog.Config>();
                    if (config == null)
                    {
                        MessageBox.Show("无效的配置文件");
                        return;
                    }
                    render(config);
                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://cron.qqe2.com/");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.ShowNewFolderButton = true;
                dlg.Description = "指定保存备份的本地文件夹";
                dlg.SelectedPath = "d:\\data\\sql\\autobackup";
                dlg.RootFolder = Environment.SpecialFolder.MyComputer;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    tbPath.Text = dlg.SelectedPath;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new QDBDog.SqlSugarHelper($"server={tbServer.Text};Initial Catalog=master;Integrated Security=True;Connection Timeout=5;").DoOne(client =>
            {
                try
                {
                    var dt = client.Ado.GetDataTable(" SELECT name FROM   sys.databases WHERE    state = 0 order by name");
                    tbNames.Text = string.Join(",", dt.Select().Select(r => r[0].ToString()).ToArray());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("将为当前指定的数据库设置备份加密证书，启用证书后，备份文件将无法在未使用特定证书的服务器上还原，是否继续？", "备份配置", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
            //检查指定目录下有无证书
            string certFile = System.IO.Path.Combine(Application.StartupPath, "sql\\AutoBakCert.cer");
            string pkFile = System.IO.Path.Combine(Application.StartupPath, "sql\\AutoBakCert.pkey");
            if (!System.IO.File.Exists(certFile) || !System.IO.File.Exists(pkFile))
            {
                MessageBox.Show("加密证书不存在！操作失败");
                return;
            }
            string sqlFile = string.Format(Resources.AutoBakCert, System.IO.Path.Combine(Application.StartupPath, "sql"));
            new QDBDog.SqlSugarHelper($"server={tbServer.Text};Initial Catalog=master;Integrated Security=True;Connection Timeout=5;").DoOne(client =>
            {
                try
                {
                    client.Ado.ExecuteCommand(System.Text.RegularExpressions.Regex.Replace(sqlFile, @"^\s*go\s*\n", "\n", System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }
    }
}
