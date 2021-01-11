using QDBDog.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDBDog
{
    /// <summary>
    /// 配置
    /// </summary>
    public class Config
    {
        public string DBServer { get; set; } = @".\sql2017";// @"server=.\sql2017;Initial Catalog=master;Integrated Security=True";

        public string DBNames { get; set; } = @"ZY_Cloud_Main,ZY_Cloud_BM,ZY_Cloud_Health,BandData";
        public string LocalPath { get; set; } = @"D:\data\sql\AutoBackup";
        public string ServerPath { get; set; } = @"bj\deve";
        public string Name { get; set; } = "研发测试";

        /// <summary>
        /// 备份频率
        /// </summary>
        /// <remarks></remarks>
        /// <see cref="https://cron.qqe2.com/"/>
        public string BackupCron { get; set; } = "0 0 2 1/1 * ? *"; //从1号开始，每天在2点时执行一次
        public string ClearCron { get; set; } = "0 0 0/4 * * ? *"; //每隔4小时执行一次
        public string FtpCron { get; set; } = "0 30 2/4 * * ? *"; //每2:30开始，每4小时执行一次

        public string ExpireType { get; set; } = "Day";
        public int ExpireValue { get; set; } = 3;


        /// <summary>
        /// 从配置文件中读取
        /// </summary>
        /// <param name="allowNew"></param>
        /// <returns></returns>
        public static Config LoadConfig(string jsonFile, bool allowNew = true)
        {
            Config config = new Config();
            if (System.IO.File.Exists(jsonFile))
            {
                string s = System.IO.File.ReadAllText(jsonFile, Encoding.UTF8);
                config = s.De<Config>();
            }
            else if (!allowNew)
            {
                throw new Exception($"未找到配置文件{jsonFile}");
            }
            return config;
        }

    }
}
