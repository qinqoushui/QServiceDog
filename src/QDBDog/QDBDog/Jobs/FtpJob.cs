using QCommon.Service.Jobs;
using QDBDog.Properties;
using QDBDog.Share;
using System;
using System.Collections.Generic;
using System.Text;


namespace QDBDog
{
    /// <summary>
    /// 上传备份文件
    /// </summary>
    public class FtpJob : ConfigJob<string>
    {
        public FtpJob() : base(nameof(FtpJob))
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
            string ftpScript = QCommon.Service.FileHelper.GetAbsolutePath("config\\dbBack.ffs_batch");
            string ftpExe = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "FreeFileSync\\FreeFileSync.exe");
            return BackupHelper.Instance.FtpFiles(config, ftpExe, ftpScript);
        }

        protected override bool doPre()
        {
            return base.doPre();
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