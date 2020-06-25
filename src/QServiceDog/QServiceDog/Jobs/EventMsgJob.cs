using Q.DevExtreme.Tpl;
using Q.Helper;
using QServiceDog.BLL;
using QServiceDog.Helpers;
using QServiceDog.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Unity.Injection;

namespace QServiceDog.Jobs
{

    /// <summary>
    /// 同步
    /// </summary>
    public class EventMsgJob : MyJobBase<string>
    {
        public EventMsgJob() : base(nameof(EventMsgJob), true)
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
                case nameof(EventPushRecord):
                    return PushEventInfo();
                default:
                    break;
            }

            return ("skip", "");
        }

        (string result, string error) PushEventInfo()
        {
            int m = GetValueInRange("minutes", 3, 120, 10);
            var list = EventBLL.Instance.FetchNeedPushListByTime(DateTime.Now.AddMinutes(0 - m), DateTime.Now);
            var senderList = EventBLL.Instance.FetchSenderList();
            int c = 0;
            //邮件一次发送，分组处理
            if (list?.Count > 0)
            {
                MsgHelper msgHelper = new MsgHelper();
                //调用推送者推送
                foreach (var g in list.GroupBy(r=>r.EventInfo.Id))
                {
                    try
                    {
                        msgHelper.Send(g.ToList(), senderList);
                        c++;
                    }
                    catch (Exception ex)
                    {
                        logger.Error("SendMsg", ex);
                        continue;
                    }
                }

                using (var ef = new ServiceDBContext())
                {
                    ef.EventPushRecord.UpdateRange(list);
                    ef.SaveChanges();
                }
                return ("succ", $"send:{c}/{list.Count}");
            }
            else
                return ("skip", "无数据");
        }

        protected override bool doPre()
        {
            return true;
        }

        protected override IList<string> getJobs(out int max, out int total)
        {
            var ss = new string[] { nameof(EventPushRecord) };
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
