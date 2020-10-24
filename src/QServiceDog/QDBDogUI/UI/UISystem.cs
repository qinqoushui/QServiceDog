using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QDBDog;
using QDBDogUI.Properties;
using QDBDog.Share;

namespace QDBDogUI.UI
{
    public partial class UISystem : UIBase
    {
        public UISystem()
        {
            InitializeComponent();
            theTableLayoutPanel = tableLayoutPanel1;
            btnDefault.Click += (s, e) => loadDefault();
            btnSave.Click += (s, e) => save();
            btnLoad.Click += (s, e) => loadConfigFromFile();
        }

        protected override void render(Config config)
        {
            base.render(config);
            var data = config.ToDict();
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

        protected override void collectSub(Dictionary<string, string> data)
        {
            base.collectSub(data);
            foreach (var control in panel1.Controls)
            {
                var ac = control as RadioButton;
                if (ac != null && ac.Tag != null && ac.Checked)
                {
                    data["ExpireType"] = ac.Tag.ToString();
                }
            }
            data["Name"] = tbName.Text;
            data["ExpireValue"] = numExpire.Value.ToString("0");
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

        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://cron.qqe2.com/");
        }

        #region FTP客户端处理

        protected string ftpInstallExe = System.IO.Path.Combine(Application.StartupPath, "Tools\\FreeFileSyncSetup.exe");
        protected string ftpExe = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "FreeFileSync\\FreeFileSync.exe");
        protected string ftpScript = System.IO.Path.Combine(Application.StartupPath, "config\\dbBack.ffs_batch");
        private void button2_Click(object sender, EventArgs e)
        {
            //运行freesync
            if (System.Diagnostics.Process.GetProcessesByName("FreeFileSync").Length > 0)
            {
                MessageBox.Show("已在运行中!!!");
                return;
            }
            System.Diagnostics.Process.Start(ftpExe);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (System.Diagnostics.Process.GetProcessesByName("FreeFileSyncSetup").Length > 0)
            {
                MessageBox.Show("安装已在进行中!!!\r\n请完成安装向导，不要修改安装目录!");
                return;
            }
            bool needInstall = true;
            if (System.IO.File.Exists(ftpExe) && MessageBox.Show("是否重新安装？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                needInstall = false;
            }
            if (needInstall)
            {
                var ps = System.Diagnostics.Process.GetProcessesByName("FreeFileSync_x64");
                foreach (var p in ps)
                    p.Kill();
                ps = System.Diagnostics.Process.GetProcessesByName("FreeFileSync");
                foreach (var p in ps)
                    p.Kill();
                MessageBox.Show("请完成安装向导\r\n不要修改安装目录!!!");
                System.Diagnostics.Process.Start(ftpInstallExe);
                //锁定按钮10秒
                lockButton(20, sender as Button);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //重新生成脚本
            bool needInstall = true;
            if (System.IO.File.Exists(ftpScript) && MessageBox.Show("是否重新生成？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                needInstall = false;
            }
            if (needInstall)
            {
                var config = loadConfig();
                string ss = Encoding.UTF8.GetString(Resources.dbBack).Replace("@localPath@", config.LocalPath).Replace("@serverPath@", config.ServerPath);
                System.IO.File.WriteAllText(ftpScript, ss, Encoding.UTF8);
                MessageBox.Show("OK");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //运行脚本
            if (System.Diagnostics.Process.GetProcessesByName("FreeFileSync").Length > 0)
            {
                MessageBox.Show("已在运行中!!!");
                return;
            }
            if (!System.IO.File.Exists(ftpScript))
            {
                MessageBox.Show("脚本尚未生成");
                return;
            }
            //临时显示界面 <ProgressDialog Minimized="true" AutoClose="true"/>
            string testFile = ftpScript + "_test.ffs_batch";
            System.IO.File.WriteAllText(testFile, System.IO.File.ReadAllText(ftpScript, Encoding.UTF8).Replace(@"<ProgressDialog Minimized=""true"" AutoClose=""true""/>", @"<ProgressDialog Minimized=""false"" AutoClose=""false""/>"), Encoding.UTF8);
            lockButton(15, sender as Button);
            MessageBox.Show("请观察运行效果,点击确定开始运行");
            var p = System.Diagnostics.Process.Start(ftpExe, testFile);
            p.WaitForExit();
            p.Dispose();
        }

        #endregion
    }
}
