using System;
using System.Collections.Generic;
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
            //互斥
            bool runone;
            System.Threading.Mutex run = new System.Threading.Mutex(true, $"{nameof(QDBDogUI)}", out runone);
            if (runone)
            {
                run.ReleaseMutex();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                QCommon.UI.Common.Setting.Default();
                QCommon.UI.Common.Setting.CopyRight = "Copyright © 2020 By 深云智创  All Rights Reserved";

                Properties.Settings.Default.Save();
                Application.Run(new FormX());
            }
            else
            {
                MessageBox.Show("程序已启动，请不要重复启动");
            }
        }
    }
}
