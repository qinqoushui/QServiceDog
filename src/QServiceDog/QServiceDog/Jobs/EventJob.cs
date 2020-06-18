using Q.Helper;
using QServiceDog.BLL;
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
        protected override void doAfter()
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
            int m = GetValueInRange("checkPowerOff", 3, 20, 10);  //检查时间，如果开机时间过早则忽略检查
#if DEBUG
            DateTime checkTime = DateTime.Now.AddMonths(-2);// DateTime.Now.AddMinutes(0 - m);
            logTime = checkTime; //调试使用
#else
            DateTime checkTime =   DateTime.Now.AddMinutes(0 - m);
#endif

            if (checkTime < powerTime)
            {
                //开机N分钟内，判断是否异常关机
                string query = $"*[System/EventID=6008]  and *[System/TimeCreated/@SystemTime>='{logTime.ToString("s")}']";
                var q = new EventLogQuery("System", PathType.LogName, query);
                EventLogReader elr = new EventLogReader(q);
                EventRecord entry;
                while ((entry = elr.ReadEvent()) != null && entry.TimeCreated.HasValue)
                {
                    // entry.Bookmark //设置书签
                    string info = $"{entry.MachineName}意外关机，时间{entry.Properties[1].Value} {entry.Properties.First().Value}";
                    EventBLL.Instance.AddEvent(new Models.EventInfo()
                    {
                        Id = Guid.NewGuid(),
                        Msg = info,
                        Time = entry.TimeCreated.Value,
                        Type = "PowerOff"

                    });
                    //写入告警列表
                    return ("succ", info);
                }
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
            var ss = new string[] { "PowerOff" };
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
