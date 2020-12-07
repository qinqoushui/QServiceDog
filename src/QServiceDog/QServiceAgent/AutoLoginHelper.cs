using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace QServiceAgent
{
    public class AutoLoginHelper
    {

        static AutoLoginHelper _ = new AutoLoginHelper();
        public static AutoLoginHelper Instance { get; } = _;


        public bool IsAutoLogin(out string userName)
        {
            var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon");

            if (reg != null && reg.GetValue("AutoAdminLogon", "0").ToString() == "1")
            {
                userName = reg.GetValue("DefaultUserName").ToString();
                return true;
            }
            else
            {

                userName = string.Empty;
                return false;
            }
        }

        public string SetAutoLogin(string user, string pwd)
        {

            var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true);
            if (reg != null)
            {
                reg.SetValue("AutoAdminLogon", "1");
                reg.SetValue("DefaultUserName", user);
                reg.SetValue("DefaultPassword", pwd);
                return user;
            }
            else
                return "未找到Winlogon";
            //不支持设置：已测试WIN2016
        }

        /// <summary>
        /// 添加开机锁定 
        /// </summary>
        public void Lock(int minute = 3)
        {
            //string taskName = "lock_watch";
            //var command = new string[]{
            //$"schtasks /delete /tn  {taskName} /F " ,
            //$@"schtasks /create /sc ONLOGON  /tn  {taskName} /tr ""cmd /c  \""%windir%\system32\rundll32.exe user32.dll,LockWorkStation \""  ""  ",
            //"exit"
            //};
            string output;
            //exec(command, out output);
            //判断启动时间是否在指定时间范围内，并锁定 
            if (DateTime.Today.AddMinutes(minute)>DateTime.Today.AddMilliseconds( Environment.TickCount))
            {
                exec(new string[] {
                    @"%windir%\system32\rundll32.exe user32.dll,LockWorkStation ","exit"
                }, out output);
            }
            else
            {
                MsgHelper.Instance.Warn($"系统已启动{Math.Ceiling(DateTime.Today.AddMilliseconds(Environment.TickCount).TimeOfDay.TotalMinutes).ToString("0")}分钟，不再锁定");
            }
        }

        public void AddTask(string taskName,string cmd)
        {
            var command = new string[]{
            $"schtasks /delete /tn  {taskName} /F " ,
            $@"schtasks /create /sc minute /mo 5  /tn  {taskName} /tr ""{cmd}""  ",
            "exit"
            };
            string output;
            exec(command, out output);
        }

        bool exec(string[] command, out string output)
        {
            output = ""; //输出字符串
            if (command != null && !command.Equals(""))
            {
                Process process = new Process();//创建进程对象
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";//设定需要执行的命令
                startInfo.UseShellExecute = false;//不使用系统外壳程序启动
                startInfo.RedirectStandardInput = true;//不重定向输入
                startInfo.RedirectStandardOutput = true; //重定向输出
                startInfo.CreateNoWindow = true;//不创建窗口
                process.StartInfo = startInfo;
                try
                {
                    if (process.Start())//开始进程
                    {
                        foreach (var line in command)
                        {
                            process.StandardInput.WriteLine(line);
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

            return true;
        }
    }
}
