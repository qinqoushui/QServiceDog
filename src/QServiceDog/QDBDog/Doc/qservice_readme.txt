//用法：实现IService接口，并在program.main方法中添加 MainService.Start()



using System;
using Topshelf;

namespace QCommon.Service
{
    partial class Program
    {
        static void Main(params string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            MyApp.MainService ms = new MyApp.MainService();
            QCommon.Service.MainService.Start(ms);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ToString());
            System.IO.File.AppendAllText("err.txt", e.ToString());
        }
    }

}

namespace MyApp
{
	public class MainService : QCommon.Service.IService
    {
        static Q.Helper.LogHelper logger = new Q.Helper.LogHelper("MainService");

        public string DisplayName { get; set; } = "显示名称";
        public string ServiceName { get; set; } = "服务名称";
        public string Description { get; set; } = "备注";

        public bool Start()
        {
            try
            {
                logger.StartLogServer(ServiceName, Properties.Settings.Default.LogServer, Encoding.UTF8);
                logger.Info("LogServer:" + Properties.Settings.Default.LogServer);
                //logger.StartLogDatabase(ServiceName, Properties.Settings.Default.ConnectionString);

				//在此实现业务
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
