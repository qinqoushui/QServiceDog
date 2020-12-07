using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace QServiceAgent
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Threading.ThreadPool.SetMinThreads(5, 10);
            System.Threading.ThreadPool.SetMaxThreads(25, 50);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            FileStream fs = null;
            try
            {
                using (fs = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), $"{nameof(QServiceAgent)}.lck"),
                      FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                {
                    try
                    {
                        string userName;

                        bool isAutoLogin = AutoLoginHelper.Instance.IsAutoLogin(out userName);
                        if (isAutoLogin)
                        {
                            MsgHelper.Instance.Warn($"已启用{userName}自动登录操作系统");
                        }
                        else
                        {
                            //明文存储密码不安全，使用微软的工具进行设置
                            System.Diagnostics.Process.Start("Autologon64.exe").WaitForExit();
                            //new Form1().ShowDialog();
                        }
                        //先行锁定系统
                        AutoLoginHelper.Instance.Lock();
                        //创建自我守护
                        AutoLoginHelper.Instance.AddTask("watch_serviceAgent", Application.ExecutablePath);
                        MsgHelper.Instance.Warn("已创建计划任务守护");
                        //启动http服务
                        new HttpServer().Start();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }

            }
            catch (IOException ex)
            {
                MsgHelper.Instance.Warn("已有一个程序正在运行");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
 
    }


}
