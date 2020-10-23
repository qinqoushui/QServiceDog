1.实现一个Job
```
using System;
using System.Collections.Generic;
using System.Text;


namespace QCommon.Service.Jobs
{
    
    public class SampleJob : QCommon.Service.Jobs.QuartzBase<string>
    {
        AutoMapper.IMapper _mapper;
        public SampleJob(AutoMapper.IMapper mapper ) : base(nameof(SampleJob) ,true)
        {
            _mapper = mapper;
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
             var ss=new string[] { "DBJob" };
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
```
1. 在config目录下创建配置文件JobConfig.xml,注意文件格式 为UTF8,并设置为"如果较新则复制",每个任务都需要一个独立的触发器
```
<?xml version="1.0" encoding="UTF-8"?>

<job-scheduling-data xmlns="http://quartznet.sourceforge.net/JobSchedulingData"
        xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
 				version="2.0">

  <processing-directives>
    <overwrite-existing-data>true</overwrite-existing-data>
  </processing-directives>

  <schedule>


    <job>
      <name>DBJob </name>
      <group>Sync</group>
      <description>同步基础数据</description>
      <job-type>TAYC.Gate.VisitorService.Jobs.GetDeviceInfo, TAYC.Gate.VisitorService</job-type>
      <durable>true</durable>
      <recover>false</recover>
    </job>
     
    <trigger>
      <cron>
        <name>tri_getDB</name>
        <group>Sync</group>
        <description>同步基础数据</description>
        <job-name>DBJob</job-name>
        <job-group>Sync</job-group>
        <start-time>2019-01-01T00:00:00.0Z</start-time>
        <misfire-instruction>SmartPolicy</misfire-instruction>
        <cron-expression>0 0/30 * * * ?</cron-expression>
      </cron>

    </trigger>
    
    <!--<trigger>
      <simple>
        <name>tri_sync</name>
        <group>Sync</group>
        <description>同步数据</description>
        <job-name>Sync</job-name>
        <job-group>Sync</job-group>
        <start-time>2019-01-01T00:00:00.0Z</start-time>
        <end-time>2050-12-31T23:59:59.0Z</end-time>
        <misfire-instruction>SmartPolicy</misfire-instruction>
        <repeat-count>-1</repeat-count>
        <repeat-interval>60000</repeat-interval>
      </simple>
    </trigger>-->
 

    
    
  </schedule>

</job-scheduling-data>
```
 

1. 程序或服务启动时，注册Job
```
  //注册定时任务
        new QCommon.Service.Jobs.QuartzJobScheduler().RunAsync();
```
1. IOC注入
```
public void ConfigureServices(IServiceCollection services)
        {
            //注入 Quartz调度类
            services.AddSingleton<QuartzJobScheduler>();
            services.AddTransient<UserInfoSyncjob>();      // 注入某个JOB
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();//注册ISchedulerFactory的实例。
            services.AddSingleton<IJobFactory, IOCJobFactory>(); //IOCJob工厂会从IOC中取JOB
        }
 public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,IApplicationLifetime appLifetime )
        {
            var quartz = app.ApplicationServices.GetRequiredService<QCommon.Service.Jobs.QuartzJobScheduler>();
            var appLifetime = app.ApplicationServices.GetRequiredService<Microsoft.Extensions.Hosting.IHostApplicationLifetime>();

            appLifetime.ApplicationStarted.Register(() =>
            {
                quartz.RunAsync().Wait(); //网站启动完成执行
            });
            appLifetime.ApplicationStopped.Register(() =>
            {
                quartz.StopAsync();  //网站停止完成执行

            });
             ...
        }
```