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
using System.Runtime.InteropServices;
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
        [DllImport("kernel32.dll")]
        private static extern bool SetLocalTime(ref SYSTEMTIME time);

        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEMTIME
        {
            public short year;
            public short month;
            public short dayOfWeek;
            public short day;
            public short hour;
            public short minute;
            public short second;
            public short milliseconds;
        }

        /// <summary>
        /// 设置系统时间
        /// </summary>
        /// <param name="dt">需要设置的时间</param>
        /// <returns>返回系统时间设置状态，true为成功，false为失败</returns>
        public static bool SetDate(DateTime dt)
        {
            SYSTEMTIME st;

            st.year = (short)dt.Year;
            st.month = (short)dt.Month;
            st.dayOfWeek = (short)dt.DayOfWeek;
            st.day = (short)dt.Day;
            st.hour = (short)dt.Hour;
            st.minute = (short)dt.Minute;
            st.second = (short)dt.Second;
            st.milliseconds = (short)dt.Millisecond;
            bool rt = SetLocalTime(ref st);
            return rt;
        }


        /// <summary>
        /// 同步基础数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override (string result, string error) doJob(string data)
        {
            string url = jobDataMap.GetString("url");
            switch (data)
            {
                case nameof(EventInfo):
                    int m = GetValueInRange("minutes", 3, 120, 10);
                    return sync(url + "Event", EventBLL.Instance.FetchListByTime(DateTime.Now.AddMinutes(0 - m), DateTime.Now));
                case nameof(ServiceInfo):
                    return sync(url + "Service", ServiceBLL.Instance.FetchList(false));
                case "SyncTime": //时差过大时，校时
                    var sResult = sync(url + "SyncTime", new List<string>() { DateTime.Now.ToString("F")});
                    if (sResult.result == "succ")
                    {
                        try
                        {
                            DateTime time;
                            if (DateTime.TryParse(sResult.error.DeserializeAnonymousType(new { Code = 0, Msg = "" }).Msg, out time))
                            {
                                int a = GetValueInRange("syncTime", 5, 30, 10);
                                if (Math.Abs(time.Subtract(DateTime.Now).TotalMinutes) > a)
                                {
                                    var f = SetDate(time) ? "成功" : "失败";
                                    return ("succ", $"校时{f},{time.ToString("F")}，now is {DateTime.Now.ToString("F")}");
                                }
                                else
                                {
                                    return ("succ", $"不必校时,{time.ToString("F")}，now is {DateTime.Now.ToString("F")}");
                                }
                            }
                            else
                            {
                                return ("fail", $"时间无效,{sResult.error}");
                            }
                        }
                        catch (Exception ex)
                        {
                            return ("fail", $"异常,{ex}");
                        }
                    }
                    else
                        return sResult;
                default:
                    break;
            }

            return ("skip", "");
        }

        (string result, string error) sync(string url, System.Collections.IList list)
        {
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





        protected override IList<string> getJobs(out int max, out int total)
        {
            var ss = new string[] { nameof(EventInfo), nameof(ServiceInfo), "SyncTime" };
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
