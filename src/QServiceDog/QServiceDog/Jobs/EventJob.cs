using Q.Helper;
using QServiceDog.BLL;
using QServiceDog.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using Unity.Injection;

namespace QServiceDog.Jobs
{
    public abstract class MyJobBase<T> : QCommon.Service.Jobs.QuartzBase<T>
    {
        public MyJobBase(string name, bool skipWhenExecuting) : base(name, skipWhenExecuting)
        {

        }

        protected override bool doPre()
        {
#if DEBUG
            return true;
#else
            bool isCloud = GlobalConfig.Instance.Client.StartsWith("Cloud", StringComparison.OrdinalIgnoreCase);
            if (isCloud)
            {
                //禁用本地检测服务
                switch (JobName)
                {
                    case nameof(EventJob):
                    case nameof(ServiceJob):
                    case nameof(SyncEventJob):
                        return false;
                    default:
                        return true;
                }
            }
            else
            {
                //禁用云模式
                switch (JobName)
                {
                    case nameof(EventMsgJob):
                        return false;
                    default:
                        return true;
                }
            }
#endif
        }
        protected int GetValueInRange(string key, int min, int max, int defaultValue)
        {
            if (jobDataMap.ContainsKey(key))
            {
                int v = jobDataMap.GetInt(key);
                if (v < min || v > max)
                    return defaultValue;
                else
                    return v;
            }
            else
                return defaultValue;
        }
    }
    /// <summary>
    /// 扫描各个服务或程序，判断是否需要启动
    /// </summary>
    public class EventJob : MyJobBase<string>
    {
        public EventJob() : base(nameof(EventJob), true)
        {

        }
        protected override void doAfter(IList<string> data)
        {

        }

        /// <summary>
        /// 同步基础数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override (string result, string error) doJob(string data)
        {
            switch (data)
            {
                case "PowerOff":
                    return checkPowerOff();
                case nameof(Q.Helper.LogHelper.ClearLog):
                    LogHelper.ClearLog(DateTime.Today.AddDays(-7));
                    return ("succ", "");
                default:
                    break;
            }

            return ("skip", "");
        }

        /// <summary>
        /// 判断异常关机
        /// </summary>
        /// <returns></returns>
        ///<see cref="https://docs.microsoft.com/en-us/previous-versions/bb399427(v=vs.90)?redirectedfrom=MSDN"/>
        ///<see cref="https://searchcode.com/file/116713581/TaskService/Native/EventLog.cs/"/>
        (string result, string error) checkPowerOff()
        {
            DateTime powerTime = DateTime.Now.AddMilliseconds(0 - Environment.TickCount64); //开机时间
            DateTime logTime = powerTime;
            int m = GetValueInRange("checkPowerOff", 15, 120, 60);  //检查时间，如果开机时间过早则忽略检查
#if DEBUG
            DateTime checkTime = DateTime.Now.AddMonths(-2);
            logTime = checkTime; //调试使用
#else
            DateTime checkTime =   DateTime.Now.AddMinutes(0 - m);
#endif

            if (checkTime <= powerTime) //检查一段时间后，开机很久了就不检查了
            {
                //开机N分钟内，判断是否异常关机
                //(windows 2016神经病的时间表示‎2020/‎8/‎30多出数据 LEFT-TO-RIGHT MARK e2 80 8e 32 30 32 30 2f e2 80 8e 33 2f e2 80 8e 32 36)
                //string query = $"*[System/EventID=6008]  and *[System/TimeCreated/@SystemTime>='{logTime.ToString("s")}']";
                string query = $"*[System/EventID=6008]"; //查询所有数据
                var q = new EventLogQuery("System", PathType.LogName, query);
                EventLogReader elr = new EventLogReader(q);
                elr.Seek(System.IO.SeekOrigin.End, 0); //移至最后
                EventRecord entry;
                //取最后一次的关机记录
                string info = string.Empty;
                string ccc = System.Text.Encoding.UTF8.GetString(new byte[] { 0xe2, 0x80, 0x8e }); //移除特殊的字符
                while ((entry = elr.ReadEvent()) != null && entry.TimeCreated.HasValue)
                {
                    if (entry.TimeCreated.Value > powerTime) //启动之后创建的事件才有效
                        info = $"{entry.MachineName}意外关机，时间{entry.Properties[1].Value.ToString().Replace(ccc, "")} {entry.Properties.First().Value}";
                }
                if (!string.IsNullOrEmpty(info))
                {
                    EventBLL.Instance.AddEvent(new Models.EventInfo()
                    {
                        Id = Guid.NewGuid(),
                        Msg = info,
#if DEBUG
                        Time = DateTime.Now, //观察
#else
                        Time = entry.TimeCreated.Value,
#endif
                        Type = "PowerOff"

                    }, true);
                    //写入告警列表
                    return ("succ", info);
                }
                else
                    return ("succ", "");
            }
            else
                return ("skip", "");
        }

        protected override bool doPre()
        {
            return true;
        }

        protected override IList<string> getJobs(out int max, out int total)
        {
            var ss = new string[] { "PowerOff", nameof(Q.Helper.LogHelper.ClearLog) };
            max = ss.Length;
            total = ss.Length;
            return ss;
        }

        protected override string getSubJobName(string data)
        {
            return data;
        }
    }
}
