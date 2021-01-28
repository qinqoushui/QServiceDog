using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace QDDNS
{
    class Program
    {
        static void Main(string[] args)
        {
            new ProgramHelper<Startup>().Run(nameof(QServiceDog), "服务守护狗", args,
#if _Docker
                        true
#else
                        false
#endif
                        ,
#if DEBUG || IIS
                        true
#else
                        false
#endif
    );
        }
    }
    class ProgramHelper<TStartup> where TStartup : class
    {
        public void Run(string name, string desc, string[] args, bool docker = false, bool debug = false, string serviceTag = "service")
        {

            if (docker)
            {
                run(docker, args);
            }
            else
            {
                if (debug)
                    run(docker, args);
                else
                    Q.Helper.ServiceHelper.StartService(Directory.GetCurrentDirectory(), name, desc, System.Reflection.Assembly.GetCallingAssembly(), () =>
                    {
                        run(docker, args);
                    }, () => { run(docker, args); }, args, false, serviceTag, true);
            }
        }
        void run(bool docker, string[] args)
        {
            if (docker)
            {
                //启动前检查文件并复制
                string aPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Bak");
                string tPath = AppContext.BaseDirectory;
                void copyFolder(DirectoryInfo source, DirectoryInfo target)
                {
                    foreach (DirectoryInfo dir in source.GetDirectories())
                        copyFolder(dir, target.CreateSubdirectory(dir.Name));
                    foreach (FileInfo file in source.GetFiles())
                        if (!File.Exists(Path.Combine(target.FullName, file.Name)))
                            file.CopyTo(Path.Combine(target.FullName, file.Name), false);
                }
                if (Directory.Exists(aPath))
                {
                    //尝试复制
                    copyFolder(new DirectoryInfo(aPath), new DirectoryInfo(tPath));
                }
            }

            System.Net.ServicePointManager.DefaultConnectionLimit = 512;
            System.Threading.ThreadPool.SetMinThreads(25, 25);
            System.Threading.ThreadPool.SetMaxThreads(100, 100);

            var x = new Wi
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                x.Run();
            }
            else
            {

                if (args != null && args.FirstOrDefault(r => r.Equals("service", StringComparison.CurrentCultureIgnoreCase)) != null)
                    x.RunAsService();
                else
                {
                    x.Run();
                }
            }
        }
    }
