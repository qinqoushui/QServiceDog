using QCommon.Service.Jobs;
using QDBDog.Properties;
using QDBDog.Share;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;


namespace QDBDog
{
    /// <summary>
    /// 备份数据库
    /// </summary>
    public class BackupJob : ConfigJob<string>
    {
        public BackupJob() : base(nameof(BackupJob))
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
            switch (data)
            {
                case nameof(BackupHelper.Backupdb):
                default:
                    return BackupHelper.Instance.Backupdb(config, Resources.BackupDB, out string subPath);
            }
        }

        protected override bool doPre()
        {
            return base.doPre();
        }

        protected override IList<string> getJobs(out int max, out int total)
        {
            //所有数据库进行一次备份
            max = 1; total = 1;
            return new string[] { nameof(BackupHelper.Backupdb) };
        }

        protected override string getSubJobName(string data)
        {
            return data;
        }
    }
}