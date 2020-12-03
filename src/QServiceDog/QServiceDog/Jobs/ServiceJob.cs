using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Q.Helper;
using Q.Sockets;
using QServiceDog.BLL;
using QServiceDog.Enums;
using QServiceDog.Helpers;
using QServiceDog.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace QServiceDog.Jobs
{
    /// <summary>
    /// 扫描各个服务或程序，判断是否需要启动
    /// 云服务器不守护服务
    /// </summary>
    public class ServiceJob : QCommon.Service.Jobs.QuartzBase<ServiceInfo>
    {
        public ServiceJob() : base(nameof(ServiceJob), true)
        {

        }
        protected override void doAfter(IList<ServiceInfo> data)
        {
            using (var ef = new ServiceDBContext())
            {
                ef.ServiceInfo.UpdateRange(data);
                int c = ef.SaveChanges();
                logger.Info($"doAfter save:{c}/{data.Count}");
            }
        }

        /// <summary>
        /// 同步基础数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override (string result, string error) doJob(ServiceInfo data)
        {

            if (check(data))
            {
                data.LastAliveTime = DateTime.Now;
                if (DateTime.Now.Hour < 6 && data.LastStopTime.Add(data.RestartTime) < DateTime.Now) //存活太久，肥了就杀,定期自动停止
                {
                    stop(data);
                    EventBLL.Instance.AddEvent(new EventInfo()
                    {
                        Id = Guid.NewGuid(),
                        Time = DateTime.Now,
                        Type = "Auto",
                        Msg = $"运维自检，主动停止{data.Desc}，上次停止于{data.LastStopTime}"
                    }, false);
                    data.LastStopTime = DateTime.Now;
                    return ("succ", $"stop at {DateTime.Now}");
                }
                else
                    return ("succ", $"alive at {DateTime.Now}");
            }
            else
            {
                if (data.LastAliveTime.Add(data.IdleTime) < DateTime.Now) //沉默太久，冒个泡,限6点前
                {
                    stop(data);
                    System.Threading.Thread.Sleep(10000);
                    run(data);
                    //再次检查
                    EventBLL.Instance.AddEvent(new EventInfo()
                    {
                        Id = Guid.NewGuid(),
                        Time = DateTime.Now,
                        Type = "Monitor",
                        Msg = $"启动{data.Desc}，上次运行于{data.LastAliveTime}。如果连续多次启动，则启动可能一直不成功"
                    }, false);
                    return ("succ", $"run at {DateTime.Now}");
                }
                else
                    return ("succ", $"idle at {DateTime.Now}");
            }
        }



        bool check(ServiceInfo data)
        {
            DogAction action = getAction(data.CheckName, out EnumAction enumAction);
            if (action != null)
            {
                switch (enumAction)
                {
                    case EnumAction.e打开网页:
                        try
                        {
                            return new HttpClient().GetAsync(data.CheckData).GetAwaiter().GetResult().StatusCode == HttpStatusCode.OK;
                        }
                        catch (Exception ex)
                        {
                            return false;
                        }
                    case EnumAction.e检测服务状态:
                        return new QCommon.Service.ServiceHelper(data.CheckData).Status == System.ServiceProcess.ServiceControllerStatus.Running;

                    case EnumAction.e检测端口:
                        return NetHelper.Telnet(IPEndPoint.Parse(data.CheckData));

                    case EnumAction.e检测IP:
                        var ping = NetHelper.Ping(data.CheckData, (s, ex) => logger.Warn(s, ex));
                        if (ping == System.Net.NetworkInformation.IPStatus.Success)
                            return true;
                        else
                            return false;

                    case EnumAction.e查找进程:
                        return ProcessHelper.Exists(data.CheckData);
                    default:
                        //CommandHelper.execute($"{action.Command} {data.CheckData}", out info, out error);
                        break;
                }
                return false;
            }
            else
            {
                logger.Warn($"未知的检测方法：{data.CheckName}");
                return false;
            }
        }

        bool stop(ServiceInfo data)
        {
            //Stop KillProcess sc stop redis
            //目前都是命令，后续可能是代码
            DogAction action = getAction(data.StopName, out EnumAction enumAction);
            bool flag = false;
            if (action != null)
            {
                logger.Info($"stop {data.Name} begin");
                switch (enumAction)
                {
                    case EnumAction.e终止进程:
                        flag = ProcessHelper.Kill(data.StopData);
                        break;
                    case EnumAction.e停止服务:
                        flag = new QCommon.Service.ServiceHelper(data.StopData).Stop();
                        break;
                    default:
                        flag = false;
                        break;
                }
                logger.Info($"stop {data.Name} end,{flag}");
                return flag;
            }
            else
            {
                logger.Warn($"未知的停止方法：{data.StopName}");
                return false;
            }
        }

        bool run(ServiceInfo data)
        {
            DogAction action = getAction(data.RunName, out EnumAction enumAction);
            if (action != null)
            {
                logger.Info($"run {data.Name} begin");
                switch (enumAction)
                {
                    case EnumAction.e启动进程:
                        new Thread(new ThreadStart(() => ProcessHelper.Start(data.RunData, (s, ex) => logger.Info(s, ex)))).Start();
                        break;
                    case EnumAction.e启动服务:
                        new QCommon.Service.ServiceHelper(data.RunData).Start();
                        break;
                }
                logger.Info($"run {data.Name} end ");
                return true;
            }
            else
            {
                logger.Warn($"未知的停止方法：{data.StopName}");
                return false;
            }
        }

        List<DogAction> dogActions = new List<DogAction>();
        protected override bool doPre()
        {
            dogActions = DogActionBLL.Instance.FetchList();
            return true;
        }
        DogAction getAction(string name, out EnumAction enumAction)
        {
            var x = dogActions.FirstOrDefault(r => r.Name == name);
            enumAction = EnumAction.e未知;
            if (x != null)
            {
                Enum.TryParse<EnumAction>("e" + name, out enumAction);
            }
            return x;
        }
        protected override IList<ServiceInfo> getJobs(out int max, out int total)
        {
            List<ServiceInfo> result = new List<ServiceInfo>();
            using (var ef = new ServiceDBContext())
            {
                //仅守护本地服务
                result = ef.ServiceInfo.Where(r => r.IsEnable && r.Client == GlobalConfig.Instance.Client).ToList();
            }
            max = result.Count;
            total = result.Count;
            return result;
        }

        protected override string getSubJobName(ServiceInfo data)
        {
            return data.Name;
        }



    }
}
