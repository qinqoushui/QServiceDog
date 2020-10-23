using System;
using System.Collections.Generic;
using System.Text;


namespace QCommon.Service.Jobs
{
    
    public class SampleJob : QCommon.Service.Jobs.QuartzBase<string>
    {
        public SampleJob() : base(nameof(SampleJob))
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
            //做业务
            if (true)
            {
                return ("succ", "");
            }
            else
                return ("fail", "");
        }

        protected override bool doPre()
        {
            return true;
        }

        protected override IList<string> getJobs(out int max, out int total)
        {
            max = 1; total = 1;
            return new string[] { "DBJob" };
        }

        protected override string getSubJobName(string data)
        {
            return data;
        }
    }
}