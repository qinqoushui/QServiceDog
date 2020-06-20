/************************************************************************************
 * Copyright (c) 2018 江苏同安云创智能系统集成科技有限公司 All Rights Reserved.
 *
 * 机器名称：FREEMANI5
 * 公司名称：江苏同安云创智能系统集成科技有限公司
 * 文件名：  ServiceController.cs
 * 版本号：  V1.0.0.0
 * 唯一标识：bf4b6eb6-560c-4340-bf71-9e6cc3226ee4
 * 用户域：  FREEMANI5
 * 创建人：  秋随
 * 电子邮箱：zhangwenxiang@jstayc.com 
 * 创建时间：2018年3月20日 10:07:34
 * 描述：    服务控制帮助
 * 
 * 
 * =====================================================================
 * 修改标记 
 * 修改时间：2018年3月20日 10:07:34
 * 修改人： 秋随
 * 版本号： V1.0.0.1
 * 描述：
 * 
 * 
 * 
 * 
 ************************************************************************************/

using System;
using System.Linq;
using System.ServiceProcess;

namespace QCommon.Service
{
    public class ServiceController
    {
        System.ServiceProcess.ServiceController sc = null;
        public string ServiceName { get; private set; }

        public ServiceControllerStatus Status
        {
            get
            {
                if (sc != null)
                    return sc.Status;
                else
                    return ServiceControllerStatus.Stopped;
            }
        }

        public ServiceController(string name)
        {
            sc = new System.ServiceProcess.ServiceController(name);
            ServiceName = name;
        }



        public bool Resume()
        {
            ServiceControllerStatus st = sc.Status;
            switch (st)
            {
                case ServiceControllerStatus.Paused:
                case ServiceControllerStatus.PausePending:
                    sc.Continue();
                    sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 30));
                    st = sc.Status;//再次获取服务状态
                    return st == ServiceControllerStatus.Running;
                case ServiceControllerStatus.Running:
                case ServiceControllerStatus.StartPending:
                    return true;
                default:
                    return false;
            }
        }


        public bool Start()
        {
            ServiceControllerStatus st = sc.Status;
            switch (st)
            {
                case ServiceControllerStatus.StopPending:
                case ServiceControllerStatus.Stopped:
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 30));
                    st = sc.Status;//再次获取服务状态
                    return st == ServiceControllerStatus.Running;
                case ServiceControllerStatus.Running:
                case ServiceControllerStatus.StartPending:
                    return true;
                default:
                    return false;
            }
        }
        public bool Stop()
        {
            ServiceControllerStatus st = sc.Status;
            switch (st)
            {
                case ServiceControllerStatus.Running:
                case ServiceControllerStatus.StartPending:
                case ServiceControllerStatus.Paused:
                case ServiceControllerStatus.PausePending:
                case ServiceControllerStatus.ContinuePending:
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                    st = sc.Status;//再次获取服务状态
                    return st == ServiceControllerStatus.Stopped;
                case ServiceControllerStatus.Stopped:
                case ServiceControllerStatus.StopPending:
                    return true;
                default:
                    return false;
            }

        }
        public bool Pause()
        {
            ServiceControllerStatus st = sc.Status;
            switch (st)
            {
                case ServiceControllerStatus.Running:
                case ServiceControllerStatus.StartPending:
                    sc.Pause();
                    sc.WaitForStatus(ServiceControllerStatus.Paused);
                    st = sc.Status;//再次获取服务状态
                    return (st == ServiceControllerStatus.Paused);
                case ServiceControllerStatus.PausePending:
                case ServiceControllerStatus.Paused:
                    return true;
                default:
                    return false;
            }
        }


        /// <summary>
        /// 服务失败时的处理
        /// <see cref="https://docs.microsoft.com/zh-cn/previous-versions/windows/it-pro/windows-server-2008-R2-and-2008/cc742019(v%3dws.10)"/>
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 非常严格的空格
        /// sc failure msftpsvc reset= 30 actions= restart/5000
        /// sc failure dfs reset= 60 actions= reboot/30000
        /// </remarks>
        public static  bool RestartWhenFailure(string serviceName, string action= "reset= 30 actions= restart/120000")
        {
            if (IsExisted(serviceName))
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.Arguments = string.Format("/c sc failure {0}  {1}", serviceName, action);
                proc.Start();
                return true;
            }
            else
                return false;
        }


        public static bool IsExisted(string name)
        {
            System.ServiceProcess.ServiceController[] services = System.ServiceProcess.ServiceController.GetServices();
            return services.Count(r => r.ServiceName.Equals(name, StringComparison.CurrentCultureIgnoreCase)) > 0;
        }
    }
}
