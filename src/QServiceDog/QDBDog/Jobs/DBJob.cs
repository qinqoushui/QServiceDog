using QCommon.Service.Jobs;
using System;
using System.Collections.Generic;
using System.Text;


namespace QDBDog
{
    /// <summary>
    /// 备份数据库
    /// </summary>
    public class DBJob : QuartzBase<string>
    {
        public DBJob() : base(nameof(DBJob))
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
            var config = Share.Config.LoadConfig(QCommon.Service.FileHelper.GetAbsolutePath("config\\appsettings.json"), false);

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
            //所有数据库进行一次备份
            max = 1; total = 1;
            return new string[] { "DBBackup" };
        }

        protected override string getSubJobName(string data)
        {
            return data;
        }
    }
}