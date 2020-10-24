using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QDBDog;
using QDBDogUI.Properties;
using QDBDog.Share;
using System.Diagnostics;
using QouShui.DLL.Forms;

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
            lockButton(3, sender as Button);
            if (System.Diagnostics.Process.GetProcessesByName("FreeFileSync").Length > 0)
            {
                MessageBox.Show(this, "已在运行中!!!");
                return;
            }
            System.Diagnostics.Process.Start(ftpExe);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (System.Diagnostics.Process.GetProcessesByName("FreeFileSyncSetup").Length > 0)
            {
                MessageBox.Show(this,  "安装已在进行中!!!\r\n请完成安装向导，不要修改安装目录!");
                return;
            }
            bool needInstall = true;
            if (System.IO.File.Exists(ftpExe) && MessageBox.Show(this,  "是否重新安装？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
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
                MessageBox.Show(this,"请完成安装向导\r\n不要修改安装目录!!!");
                System.Diagnostics.Process.Start(ftpInstallExe);
                //锁定按钮10秒
                lockButton(20, sender as Button);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //重新生成脚本
            bool needInstall = true;
            if (System.IO.File.Exists(ftpScript) && MessageBox.Show(this,"是否重新生成？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                needInstall = false;
            }
            if (needInstall)
            {
                var config = loadConfig();
                string ss = Encoding.UTF8.GetString(Resources.dbBack).Replace("@localPath@", config.LocalPath).Replace("@serverPath@", config.ServerPath);
                System.IO.File.WriteAllText(ftpScript, ss, Encoding.UTF8);
                MessageBox.Show(this,"OK");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //运行脚本
            if (System.Diagnostics.Process.GetProcessesByName("FreeFileSync").Length > 0)
            {
                MessageBox.Show(this,"已在运行中!!!");
                return;
            }
            if (!System.IO.File.Exists(ftpScript))
            {
                MessageBox.Show(this,"脚本尚未生成");
                return;
            }
            //临时显示界面 <ProgressDialog Minimized="true" AutoClose="true"/>
            string testFile = ftpScript + "_test.ffs_batch";
            System.IO.File.WriteAllText(testFile, System.IO.File.ReadAllText(ftpScript, Encoding.UTF8).Replace(@"<ProgressDialog Minimized=""true"" AutoClose=""true""/>", @"<ProgressDialog Minimized=""false"" AutoClose=""false""/>"), Encoding.UTF8);
            lockButton(15, sender as Button);
            MessageBox.Show(this,"请观察运行效果,点击确定开始运行");
            var p = System.Diagnostics.Process.Start(ftpExe, testFile);
            p.WaitForExit();
            p.Dispose();
        }

        #endregion

        #region 服务控制

        protected string serviceExe = System.IO.Path.Combine(Application.StartupPath, "QDBDog.exe");
        private void button9_Click(object sender, EventArgs e)
        {
            lockButton(3, sender as Button);
            new FormWait().Start((sender as Button).Text, () =>
            {
                aa((sender as Button).Tag.ToString());
            });

        }
        void aa(string action)
        {
            try
            {
                switch (action)
                {
                    case "Console":
                        Process.Start(serviceExe);
                        break;
                    case "Install":
                        showResult(execute($"/c \"{serviceExe}\" install"));
                        break;
                    case "Start":
                        showResult(execute($"/c \"{serviceExe}\" start"));
                        break;
                    case "Stop":
                        showResult(execute($"/c \"{serviceExe}\" stop"));
                        break;
                    case "Uninstall":
                        showResult(execute($"/c \"{serviceExe}\" uninstall"));
                        break;
                    case "MGR":
                        //textBox4.Text=  execute($"/c services.msc", 10);
                        System.Diagnostics.Process.Start("services.msc");
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        string execute(string command, int seconds = 5)
        {
            string output = ""; //输出字符串
            if (command != null && !command.Equals(""))
            {
                Process process = new Process();//创建进程对象
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";//设定需要执行的命令
                startInfo.Arguments = "  " + command;//“/C”表示执行完命令后马上退出
                startInfo.UseShellExecute = false;//不使用系统外壳程序启动
                startInfo.RedirectStandardInput = false;//不重定向输入
                startInfo.RedirectStandardOutput = true; //重定向输出
                startInfo.CreateNoWindow = true;//不创建窗口
                process.StartInfo = startInfo;
                try
                {
                    if (process.Start())//开始进程
                    {
                        if (seconds == 0)
                        {
                            process.WaitForExit();//这里无限等待进程结束
                        }
                        else
                        {
                            process.WaitForExit(seconds * 1000); //等待进程结束，等待时间为指定的毫秒
                        }
                        output = process.StandardOutput.ReadToEnd();//读取进程的输出
                    }
                }
                catch (Exception ex)
                {
                    output = ex.ToString();
                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
            }
            return output;
        }
        #endregion


    }
}
