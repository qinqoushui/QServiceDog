using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Q.Helper;
using Q.Sockets;
using QServiceDog.BLL;
using QServiceDog.Enums;
using QServiceDog.Helpers;
using QServiceDog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QServiceDog.Jobs
{
    /// <summary>
    /// 扫描各个服务或程序，判断是否需要启动
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
                if (data.LastStopTime.Add(data.RestartTime) < DateTime.Now) //存活太久，肥了就杀
                {
                    stop(data);
                    data.LastStopTime = DateTime.Now;
                    return ("succ", $"stop at {DateTime.Now}");
                }
                else
                    return ("succ", $"alive at {DateTime.Now}");
            }
            else
            {
                if (data.LastAliveTime.Add(data.IdleTime) < DateTime.Now) //沉默太久，冒个泡
                {
                    run(data);
                    return ("succ", $"run at {DateTime.Now}");
                }
                else
                    return ("succ", $"idle at {DateTime.Now}");
            }
        }



        bool check(ServiceInfo data)
        {
            //判断服务的检测方式 Get/Post Url ,Telnet IPEndPoint ,Ping IP,GetProcess ProcessName
            DogAction action = getAction(data.CheckName, out EnumAction enumAction);
            if (action != null)
            {
                string info, error;

                switch (enumAction)
                {
                    case EnumAction.e打开网页:
                        break;
                    case EnumAction.e检测服务状态:
                        CommandHelper.execute($"{action.Command} {data.CheckData}", out info, out error);
                        //判断服务状态
                        break;
                    case EnumAction.e检测端口:
                        CommandHelper.execute($"{action.Command} {data.CheckData}", out info, out error);
                        //判断服务状态
                        break;
                    case EnumAction.e检测IP:
                        var ping = NetHelper.Ping(data.CheckData, (s, ex) => logger.Warn(s, ex));
                        if (ping == System.Net.NetworkInformation.IPStatus.Success)
                            return true;
                        else
                            return false;
                    case EnumAction.e查找进程:
                        CommandHelper.execute($"{action.Command} {data.CheckData}", out info, out error);
                        //判断服务状态
                        break;
                    default:
                        CommandHelper.execute($"{action.Command} {data.CheckData}", out info, out error);
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
            if (action != null)
            {
                logger.Info($"stop {data.Name} begin");
                switch (enumAction)
                {
                    case EnumAction.e终止进程:
                        ProcessHelper.Kill(data.StopData);
                        break;
                }

                return true;
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
                string info = string.Empty;
                string error = string.Empty;
                switch (enumAction)
                {
                    case EnumAction.e启动进程:
                        new Thread(new ThreadStart(() => ProcessHelper.Start(data.RunData, (s, ex) => logger.Info(s, ex)))).Start();
                        break;
                }
                if (!string.IsNullOrEmpty(error))
                    logger.Warn($"run {data.Name} error,{error}");
                logger.Info($"run {data.Name} end,{info}");
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
                result = ef.ServiceInfo.Where(r => r.IsEnable).ToList().Take(1).ToList();
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
