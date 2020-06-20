﻿using Q.DevExtreme.Tpl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QServiceDog.Helpers
{
    public class ProcessHelper
    {
        public static bool Exists(string processName)
        {
            return System.Diagnostics.Process.GetProcessesByName(processName).Length > 0;
        }
        public static bool Kill(string processName)
        {
            try
            {
                List<Process> p = null;
                if (processName.EndsWith("*"))
                {
                    p = Process.GetProcesses().Where(r => r.ProcessName.StartsWith(processName.Substring(0, processName.Length - 1), StringComparison.OrdinalIgnoreCase)).ToList();

                }
                else
                {
                    p = Process.GetProcessesByName(processName).ToList();
                }
                p.ForEach(r =>
                {
                    try
                    {
                        r.Kill();
                    }
                    catch (Exception ex)
                    {
                        //kill Calculator 的时候会把调试进程 也杀死？？？
                    }
                });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool Start(string data, Action<string, Exception> logger)
        {
            try
            {
                var ss = data.DeserializeAnonymousType(new { FileName = "", Para = "", WorkingPath = "" });
                Process process = new Process();//创建进程对象
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = ss.FileName;
                if (!string.IsNullOrEmpty(ss.Para))
                    startInfo.Arguments = ss.Para;
                startInfo.Verb = "RunAs";
                if (!string.IsNullOrEmpty(ss.WorkingPath))
                    startInfo.WorkingDirectory = ss.WorkingPath;
                process.StartInfo = startInfo;
                process.Start(); //创建了新进程 ，不管其死活
                process.WaitForExit();
                logger?.Invoke($"Process {data} Exit", null);
                return true;
            }
            catch (Exception ex)
            {
                logger?.Invoke($"Process {data} Error", ex);
                return false;
            }
        }
    }

    public class CommandHelper
    {
        public static void execute(string command, out string output, out string error, int seconds = 10)
        {
            output = ""; //输出字符串
            error = string.Empty;
            if (command != null && !command.Equals(""))
            {
                Process process = new Process();//创建进程对象
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";//设定需要执行的命令
                startInfo.Arguments = command;
                startInfo.Verb = "RunAs";
                startInfo.UseShellExecute = false;//不使用系统外壳程序启动
                startInfo.RedirectStandardInput = false;//不重定向输入
                startInfo.RedirectStandardOutput = true; //重定向输出
                startInfo.RedirectStandardError = true;
                startInfo.CreateNoWindow = true;//不创建窗口
                process.StartInfo = startInfo;
                StringBuilder sbInfo = new StringBuilder();
                StringBuilder sbError = new StringBuilder();
                process.OutputDataReceived += (ss, ee) => sbInfo.Append(ee.Data);
                process.ErrorDataReceived += (ss, ee) => sbError.Append(ee.Data);
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
                        output = sbInfo.ToString();// process.StandardOutput.ReadToEnd(); //卡死
                        error = sbError.ToString();// process.StandardError.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    error = ex.GetExceptionMsg();
                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
            }
        }

        public static void execute(string[] command, out string output, out string error, int seconds = 10)
        {
            output = ""; //输出字符串
            error = string.Empty;
            if (command?.Length > 0)
            {
                Process p = null;
                try
                {
                    p = Process.Start(new ProcessStartInfo()
                    {
                        FileName = "cmd.exe",
                        RedirectStandardInput = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    });
                    StringBuilder sbInfo = new StringBuilder();
                    p.OutputDataReceived += (ss, ee) => sbInfo.Append(ee.Data);
                    var i = p.StandardInput;
                    foreach (string s in command)
                    {
                        i.Write(s);
                        i.WriteLine();
                    }
                    i.Close();

                    output = sbInfo.ToString();// p.StandardOutput.ReadToEnd();//读取进程的输出
                    error = p.StandardError.ReadToEnd();
                }
                catch (Exception ex)
                {
                    output = ex.GetExceptionMsg();
                }
                finally
                {
                    if (p != null)
                        p.Close();
                }
            }
        }


        /// <summary>
        /// 启动一个新进程，并不结束它
        /// </summary>
        /// <param name="command"></param>
        /// <param name="output"></param>
        /// <param name="error"></param>
        /// <param name="seconds"></param>
        public static void start(string[] command, out string output, out string error, int seconds = 10)
        {
            output = ""; //输出字符串
            error = string.Empty;
            if (command?.Length > 0)
            {
                Process p = null;
                try
                {
                    p = Process.Start(new ProcessStartInfo()
                    {
                        FileName = "cmd.exe",
                        RedirectStandardInput = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    });
                    StringBuilder sbInfo = new StringBuilder();
                    StringBuilder sbError = new StringBuilder();
                    p.OutputDataReceived += (ss, ee) => sbInfo.Append(ee.Data);
                    p.ErrorDataReceived += (ss, ee) => sbError.Append(ee.Data);
                    var i = p.StandardInput;
                    foreach (string s in command)
                    {
                        i.Write(s);
                        i.WriteLine();
                    }
                    i.Close();

                    output = sbInfo.ToString();// p.StandardOutput.ReadToEnd();//读取进程的输出
                    error = sbError.ToString();// p.StandardError.ReadToEnd();
                }
                catch (Exception ex)
                {
                    output = ex.GetExceptionMsg();
                }
                finally
                {
                    if (p != null)
                        p.Close();
                }
            }
        }


    }
}
