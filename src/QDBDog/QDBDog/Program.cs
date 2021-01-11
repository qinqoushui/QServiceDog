using QDBDog.Share;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDBDog
{
    public class Program
    {
        static void Main(params string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            MainService ms = new MainService();
            QCommon.Service.MainService.Start(ms);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ToString());
            System.IO.File.AppendAllText("err.txt", e.ToString());
        }
    }

    public class MainService : QCommon.Service.IService
    {
        static Q.Helper.LogHelper logger = new Q.Helper.LogHelper("MainService");

        public string DisplayName { get; set; } = "数据备份工具";
        public string ServiceName { get; set; } = "QBackupDog";
        public string Description { get; set; } = "数据备份工具";

        public bool Start()
        {
            try
            {
                Q.Helper.Logging.Init(System.IO.File.ReadAllText(QCommon.Service.FileHelper.GetAbsolutePath("config\\logging.json")).De<Q.Helper.Logging>());
                logger.StartLogServer($"{Q.Helper.Logging.Instance.AppName}", Q.Helper.Logging.Instance.LogServer, Encoding.UTF8);
                logger.Info("LogServer:" + Q.Helper.Logging.Instance.LogServer);
                //启用任务
                Task.Delay(6000).ContinueWith(async t =>
                {
                    try
                    {
                        await new Jobs.QuartzJobScheduler(null, new Quartz.Impl.StdSchedulerFactory()).RunAsync();
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("JobScheduler", ex);

                    }
                });

                return true;
            }
            catch (Exception ex)
            {
                logger.Fatal("Start", ex);
                return false;
            }
        }


        public bool Stop()
        {

            return true;
        }

    }
}
