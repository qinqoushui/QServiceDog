using Q.DevExtreme.Tpl;
using Q.Helper;
using QServiceDog.BLL;
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
    public class SyncEventJob : MyJobBase<string>
    {
        public SyncEventJob() : base(nameof(SyncEventJob), true)
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
                case nameof(EventInfo):
                    return syncEventInfo();
                default:
                    break;
            }

            return ("skip", "");
        }

        (string result, string error) syncEventInfo()
        {
            int m = GetValueInRange("minutes", 3, 120, 10);
            string url = jobDataMap.GetString("eventinfo");
            var list = EventBLL.Instance.FetchListByTime(DateTime.Now.AddMinutes(0 - m), DateTime.Now);
            if (list?.Count > 0)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        //表头参数
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        var result = client.PostAsync(url, new StringContent(list.SerializeObject(), System.Text.Encoding.UTF8, "application/json")).GetAwaiter().GetResult();
                        if (result.IsSuccessStatusCode)
                        {
                            return ("succ", result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                        }
                        else
                        {
                            return ("fail", result.StatusCode.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ("fail", ex.GetExceptionMsg());
                }
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
            var ss = new string[] { nameof(EventInfo) };
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
