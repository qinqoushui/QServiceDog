using QCommon.Service.Jobs;
using QDBDog.Properties;
using QDBDog.Share;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.ServiceModel.Security;
using System.Text;


namespace QDBDog
{

    public abstract class ConfigJob<T> : QuartzBase<T>
    {
        public ConfigJob(string name) : base(name,true)
        {

        }
        protected Config config;

        protected override bool doPre()
        {
            config = Config.LoadConfig(QCommon.Service.FileHelper.GetAbsolutePath("config\\appsettings.json"), false);
            return base.doPre();
        }

    }
    /// <summary>
    /// 备份数据库
    /// </summary>
    public class ClearJob : ConfigJob<string>
    {
        public ClearJob() : base(nameof(ClearJob))
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
                case nameof(BackupHelper.ClearBackupFiles):
                    return BackupHelper.Instance.ClearBackupFiles(config);
                case nameof(Q.Helper.LogHelper.ClearLog):
                default:
                    return ("succ", Q.Helper.LogHelper.ClearLog(DateTime.Today.AddDays(GetValueInRange("logDay", -180, -3, -7))).ToString());
            }
        }

        protected override bool doPre()
        {
            return base.doPre();
        }

        protected override IList<string> getJobs(out int max, out int total)
        {
            //所有数据库进行一次备份
            var ss= new string[] { nameof(BackupHelper.ClearBackupFiles), nameof(Q.Helper.LogHelper.ClearLog) };
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