using Quartz;
using Quartz.Simpl;
using Quartz.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDBDog.Jobs
{
    public class QuartzJobScheduler
    {
        static Q.Helper.ILogHelper logger = Q.Helper.LogHelper.Default.Get(nameof(QuartzJobScheduler), "Info");

        IScheduler _scheduler = null;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly Quartz.Spi.IJobFactory _iocJobfactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iocJobfactory"></param>
        /// <param name="schedulerFactory">new StdSchedulerFactory()</param>
        public QuartzJobScheduler(Quartz.Spi.IJobFactory iocJobfactory, ISchedulerFactory schedulerFactory)
        {
            this._schedulerFactory = schedulerFactory;
            this._iocJobfactory = iocJobfactory;
        }

        public async Task RunAsync()
        {
            XMLSchedulingDataProcessor processor = new XMLSchedulingDataProcessor(new SimpleTypeLoadHelper());

            _scheduler = await _schedulerFactory.GetScheduler();

            //程序配置任务
            var config = Config.LoadConfig(QCommon.Service.FileHelper.GetAbsolutePath("config\\appsettings.json"), false);
            DateTimeOffset startTime = DateBuilder.NextGivenSecondDate(null, 15);
            await createCronJob<BackupJob>(config.BackupCron, "backup");
            await createCronJob<ClearJob>(config.ClearCron, "clear", d =>
             {
                 d.UsingJobData("logDay", "-7");
             });
            await createCronJob<FtpJob>(config.FtpCron, "ftp");

            var ks = await _scheduler.GetTriggerKeys(Quartz.Impl.Matchers.GroupMatcher<TriggerKey>.AnyGroup());
            logger.Info($"now has trigger {ks.Count}: {string.Join(",", ks.ToList().Select(r => r.Name).ToArray())}");
            await _scheduler.Start();

        }

        private async Task createCronJob<T>(string cron, string group, Action<TriggerBuilder> addJobData = null) where T : IJob
        {
            IJobDetail job = JobBuilder.Create<T>()
                            .WithIdentity(typeof(T).Name, group)
                            .Build();
            var x = TriggerBuilder.Create()
                .WithIdentity($"tri_{ typeof(T).Name}", group).StartNow()
                .WithCronSchedule(cron, f => f.WithMisfireHandlingInstructionFireAndProceed()) ;
            addJobData?.Invoke(x);
            ICronTrigger trigger = (ICronTrigger)x.Build();
            DateTimeOffset ft = await _scheduler.ScheduleJob(job, trigger);
            logger.Info($"{job.Key} will run at: {ft.ToLocalTime():F} and repeat: {trigger.CronExpressionString}");
        }

        public async System.Threading.Tasks.Task StopAsync()
        {
            await _scheduler.Shutdown(true);
            logger.Info("------- Shutdown Complete -----------------");

            SchedulerMetaData metaData = await _scheduler.GetMetaData();
            logger.Info($"Executed {metaData.NumberOfJobsExecuted} jobs.");
        }


        public int GetValueInRange(JobDataMap map, string key, int min, int max, int defaultValue)
        {
            if (map.ContainsKey(key))
            {
                int v = map.GetInt(key);
                if (v < min || v > max)
                    return defaultValue;
                else
                    return v;
            }
            else
                return defaultValue;
        }
    }
}
