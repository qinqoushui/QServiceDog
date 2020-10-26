using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QDBDogUI
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            FileStream stream = null;
            try
            {
                //互斥(fody与MUTEX冲突,改变锁定文件）
                //bool runone;
                //System.Threading.Mutex run = new System.Threading.Mutex(false, $"{nameof(QDBDogUI)}", out runone);
                stream = System.IO.File.Open(System.IO.Path.Combine(Application.StartupPath, "mutex"), System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
                //if (runone)
                //{
                //    run.ReleaseMutex();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                QCommon.UI.Common.Setting.Default();
                QCommon.UI.Common.Setting.CopyRight = "Copyright © 2020 By 深云智创  All Rights Reserved";

                Properties.Settings.Default.Save();
                Application.Run(new FormX());
                //}
                //else
            }
            catch (Exception ex)
            {
                MessageBox.Show("程序已启动，请不要重复启动");
                //  MessageBox.Show(ex.Message);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                    stream = null;
                }
            }
        }
    }
}
