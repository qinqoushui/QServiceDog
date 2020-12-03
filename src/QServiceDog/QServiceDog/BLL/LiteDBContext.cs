using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Q.DevExtreme.Tpl.Models;
using Q.DevExtreme.Tpl;
using System;
using System.Collections.Generic;
using System.Linq;
using QServiceDog.Models;

namespace QServiceDog.BLL
{
    public class ServiceDBContext : DbContext
    {

        public static string ConnectionString { get; set; }
        public ServiceDBContext()
        {
        }
        private static bool _created = false;
        public ServiceDBContext(DbContextOptions options) : base(options)
        {

        }

        object lck = new object();

        public void CreateAndInitData(string clientName)
        {
            lock (lck)
            {
                if (_created)
                    return;
                _created = true;
#if DEBUG
              //  Database.EnsureDeleted();
#endif

                if (!Database.EnsureCreated())
                    return;
                if (!User.Any())
                {
                    User.Add(new Q.DevExtreme.Tpl.Models.User()
                    {
                        Id = Guid.NewGuid(),
                        UserNo = "admin",
                        Password = Extension.makePassword("123456"),
                        UserName = "admin",
                        Role = "管理员"
                    });
                    SaveChanges();
                }
                if (!DogAction.Any())
                {
                    DogAction.Add(new DogAction()
                    {
                        Id = Guid.NewGuid(),
                        Command = "sc query",
                        Name = Enums.EnumAction.e检测服务状态.ToString().Substring(1),
                        Type = "Check"
                    });

                    DogAction.Add(new DogAction()
                    {
                        Id = Guid.NewGuid(),
                        Command = "sc stop {0}",
                        Name = Enums.EnumAction.e停止服务.ToString().Substring(1),
                        Type = "Stop"
                    });

                    DogAction.Add(new DogAction()
                    {
                        Id = Guid.NewGuid(),
                        Command = "sc start {0}",
                        Name = Enums.EnumAction.e启动服务.ToString().Substring(1),
                        Type = "Run"
                    });

                    DogAction.Add(new DogAction()
                    {
                        Id = Guid.NewGuid(),
                        Command = "Telent",
                        Name = Enums.EnumAction.e检测端口.ToString().Substring(1),
                        Type = "Check"
                    });

                    DogAction.Add(new DogAction()
                    {
                        Id = Guid.NewGuid(),
                        Command = "/c ping {0} -4 -n 2",
                        Name = Enums.EnumAction.e检测IP.ToString().Substring(1),
                        Type = "Check"
                    });

                    DogAction.Add(new DogAction()
                    {
                        Id = Guid.NewGuid(),
                        Command = "/c tasklist|findstr /i",
                        Name = Enums.EnumAction.e查找进程.ToString().Substring(1),
                        Type = "Check"
                    });

                    DogAction.Add(new DogAction()
                    {
                        Id = Guid.NewGuid(),
                        Command = "",
                        Name = Enums.EnumAction.e启动进程.ToString().Substring(1),
                        Type = "Run"
                    });

                    DogAction.Add(new DogAction()
                    {
                        Id = Guid.NewGuid(),
                        Command = "/c TASKKILL /F /IM {0}",
                        Name = Enums.EnumAction.e终止进程.ToString().Substring(1),
                        Type = "Stop"
                    });

                    DogAction.Add(new DogAction()
                    {
                        Id = Guid.NewGuid(),
                        Command = "Get",
                        Name = Enums.EnumAction.e打开网页.ToString().Substring(1),
                        Type = "Check"
                    });
                    SaveChanges();
                }
                #region 初始化模板
                if (!ServiceTpl.Any())
                {
                    ServiceTpl.Add(new ServiceTpl()
                    {
                        Id = Guid.NewGuid(),
                        Desc = "进程检测",
                        Name = "Process",
                        CheckName = Enums.EnumAction.e查找进程.ToString().Substring(1),
                        CheckData = "Notepad",
                        RunName = Enums.EnumAction.e启动进程.ToString().Substring(1),
                        RunData = new
                        {
                            FileName = @"c:\windows\system32\notepad.exe"
                        }.SerializeObject(),
                        StopName = Enums.EnumAction.e终止进程.ToString().Substring(1),
                        StopData = "notepad",
                        LastAliveTime = DateTime.Now,
                        LastStopTime = DateTime.Now,
                        IdleTime = TimeSpan.FromMinutes(5),
                        RestartTime = TimeSpan.FromDays(30),
                        Client = "通用模板",
                        IsEnable = true
                    });

                    ServiceTpl.Add(new ServiceTpl()
                    {
                        Id = Guid.NewGuid(),
                        Desc = "Url检测",
                        Name = "Url",
                        CheckName = Enums.EnumAction.e打开网页.ToString().Substring(1),
                        CheckData = "http://192.168.10.37:8080",
                        RunName = Enums.EnumAction.e启动进程.ToString().Substring(1),
                        RunData = new
                        {
                            FileName = @"c:\windows\system32\notepad.exe"
                        }.SerializeObject(),
                        StopName = Enums.EnumAction.e终止进程.ToString().Substring(1),
                        StopData = "notepad",
                        LastAliveTime = DateTime.Now,
                        LastStopTime = DateTime.Now,
                        IdleTime = TimeSpan.FromMinutes(5),
                        RestartTime = TimeSpan.FromDays(30),
                        Client = "通用模板",
                        IsEnable = true
                    });

                    ServiceTpl.Add(new ServiceTpl()
                    {
                        Id = Guid.NewGuid(),
                        Desc = "端口检测",
                        Name = "Port",
                        CheckName = Enums.EnumAction.e检测端口.ToString().Substring(1),
                        CheckData = "127.0.0.1:1433",
                        RunName = Enums.EnumAction.e启动服务.ToString().Substring(1),
                        RunData = @"MSSQL$SQL2014",
                        StopName = Enums.EnumAction.e停止服务.ToString().Substring(1),
                        StopData = "MSSQL$SQL2014",
                        LastAliveTime = DateTime.Now,
                        LastStopTime = DateTime.Now,
                        IdleTime = TimeSpan.FromMinutes(5),
                        RestartTime = TimeSpan.FromDays(30),
                        Client = "通用模板",
                        IsEnable = true
                    });

                    ServiceTpl.Add(new ServiceTpl()
                    {
                        Id = Guid.NewGuid(),
                        Desc = "服务检测",
                        Name = "Service",
                        CheckName = Enums.EnumAction.e检测服务状态.ToString().Substring(1),
                        CheckData = "redis",
                        RunName = Enums.EnumAction.e启动服务.ToString().Substring(1),
                        RunData = @"redis",
                        StopName = Enums.EnumAction.e停止服务.ToString().Substring(1),
                        StopData = "redis",
                        LastAliveTime = DateTime.Now,
                        LastStopTime = DateTime.Now,
                        IdleTime = TimeSpan.FromMinutes(1),
                        RestartTime = TimeSpan.FromMinutes(3),
                        Client = "通用模板",
                        IsEnable = true
                    });

                    ServiceTpl.Add(new ServiceTpl()
                    {
                        Id = Guid.NewGuid(),
                        Desc = "IP检测",
                        Name = "Ping",
                        CheckName = Enums.EnumAction.e检测IP.ToString().Substring(1),
                        CheckData = "192.168.255.1",
                        RunName = Enums.EnumAction.e启动进程.ToString().Substring(1),
                        RunData = new
                        {
                            FileName = @"C:\Program Files\OpenVPN\bin\openvpn.exe",
                            Para = @"""d:\data\openvpn\config\aliyun.ovpn""",
                            WorkingPath = @"d:\data\openvpn\config"
                        }.SerializeObject(),
                        StopName = Enums.EnumAction.e终止进程.ToString().Substring(1),
                        StopData = "openvpn*",
                        LastAliveTime = DateTime.Now,
                        LastStopTime = DateTime.Now,
                        IdleTime = TimeSpan.FromMinutes(1),
                        RestartTime = TimeSpan.FromMinutes(5),
                        Client = "通用模板",
                        IsEnable = true
                    });

                    #region 产品模板
                    //常用产品：门禁、消费、考勤、数据备份、手环、IIS 等
                    ServiceTpl.Add(new ServiceTpl()
                    {
                        Id = Guid.NewGuid(),
                        Desc = "门禁服务",
                        Name = "AC",
                        CheckName = Enums.EnumAction.e检测端口.ToString().Substring(1),
                        CheckData = "127.0.0.1:5000",
                        RunName = Enums.EnumAction.e启动服务.ToString().Substring(1),
                        RunData = "ZY.Cloud.Front.ACService.exe",
                        StopName = Enums.EnumAction.e停止服务.ToString().Substring(1),
                        StopData = "ZY.Cloud.Front.ACService.exe",
                        LastAliveTime = DateTime.Now,
                        LastStopTime = DateTime.Now,
                        IdleTime = TimeSpan.FromMinutes(5),
                        RestartTime = TimeSpan.FromDays(7),
                        Client = "产品模板",
                        IsEnable = true
                    });

                    ServiceTpl.Add(new ServiceTpl()
                    {
                        Id = Guid.NewGuid(),
                        Desc = "微信服务",
                        Name = "WeChat",
                        CheckName = Enums.EnumAction.e检测端口.ToString().Substring(1),
                        CheckData = "127.0.0.1:5005",
                        RunName = Enums.EnumAction.e启动服务.ToString().Substring(1),
                        RunData = "ZY.Wechat.Service.exe",
                        StopName = Enums.EnumAction.e停止服务.ToString().Substring(1),
                        StopData = "ZY.Wechat.Service.exe",
                        LastAliveTime = DateTime.Now,
                        LastStopTime = DateTime.Now,
                        IdleTime = TimeSpan.FromMinutes(5),
                        RestartTime = TimeSpan.FromDays(7),
                        Client = "产品模板",
                        IsEnable = true
                    });

                    ServiceTpl.Add(new ServiceTpl()
                    {
                        Id = Guid.NewGuid(),
                        Desc = "消费服务",
                        Name = "Pos",
                        CheckName = Enums.EnumAction.e检测端口.ToString().Substring(1),
                        CheckData = "127.0.0.1:8888",
                        RunName = Enums.EnumAction.e启动服务.ToString().Substring(1),
                        RunData = "PosService",
                        StopName = Enums.EnumAction.e停止服务.ToString().Substring(1),
                        StopData = "PosService",
                        LastAliveTime = DateTime.Now,
                        LastStopTime = DateTime.Now,
                        IdleTime = TimeSpan.FromMinutes(5),
                        RestartTime = TimeSpan.FromDays(7),
                        Client = "产品模板",
                        IsEnable = true
                    });

                    ServiceTpl.Add(new ServiceTpl()
                    {
                        Id = Guid.NewGuid(),
                        Desc = "青果教务对接",
                        Name = "QGAPI",
                        CheckName = Enums.EnumAction.e检测服务状态.ToString().Substring(1),
                        CheckData = "TAYCApiService",
                        RunName = Enums.EnumAction.e启动服务.ToString().Substring(1),
                        RunData = "TAYCApiService",
                        StopName = Enums.EnumAction.e停止服务.ToString().Substring(1),
                        StopData = "TAYCApiService",
                        LastAliveTime = DateTime.Now,
                        LastStopTime = DateTime.Now,
                        IdleTime = TimeSpan.FromMinutes(5),
                        RestartTime = TimeSpan.FromDays(7),
                        Client = "产品模板",
                        IsEnable = true
                    });

                    ServiceTpl.Add(new ServiceTpl()
                    {
                        Id = Guid.NewGuid(),
                        Desc = "数据库备份上传服务",
                        Name = "QBackup2",
                        CheckName = Enums.EnumAction.e检测服务状态.ToString().Substring(1),
                        CheckData = "QBackup2",
                        RunName = Enums.EnumAction.e启动服务.ToString().Substring(1),
                        RunData = "QBackup2",
                        StopName = Enums.EnumAction.e停止服务.ToString().Substring(1),
                        StopData = "QBackup2",
                        LastAliveTime = DateTime.Now,
                        LastStopTime = DateTime.Now,
                        IdleTime = TimeSpan.FromMinutes(5),
                        RestartTime = TimeSpan.FromDays(7),
                        Client = "产品模板",
                        IsEnable = true
                    });

                    ServiceTpl.Add(new ServiceTpl()
                    {
                        Id = Guid.NewGuid(),
                        Desc = "手环服务",
                        Name = "Band",
                        CheckName = Enums.EnumAction.e打开网页.ToString().Substring(1),
                        CheckData = "http://127.0.0.1:8080/band",
                        RunName = Enums.EnumAction.e启动进程.ToString().Substring(1),
                        RunData = "dotnet.exe",
                        StopName = Enums.EnumAction.e终止进程.ToString().Substring(1),
                        StopData = "dotnet.exe",
                        LastAliveTime = DateTime.Now,
                        LastStopTime = DateTime.Now,
                        IdleTime = TimeSpan.FromMinutes(5),
                        RestartTime = TimeSpan.FromDays(7),
                        Client = "产品模板",
                        IsEnable = true
                    });
                    #endregion

                    SaveChanges();
                }
                #endregion

                if (!EventSubscriber.Any())
                {
                    EventSubscriber.Add(new EventSubscriber()
                    {
                        Id = Guid.NewGuid(),
                        Name = "张文相",
                        WXName = "ZhangWenXiang",
                        EMail = "18001036828@189.cn",
                        IsEnable = true
                    });
                    EventSubscriber.Add(new EventSubscriber()
                    {
                        Id = Guid.NewGuid(),
                        Name = "牛家隆",
                        WXName = "NiuJiaLong",
                        EMail = "niujialong@jstayc.com",
                        IsEnable = true
                    });
                    EventSubscriber.Add(new EventSubscriber()
                    {
                        Id = Guid.NewGuid(),
                        Name = "马健",
                        WXName = "MaJian",
                        EMail = "majian@jstayc.com",
                        IsEnable = false
                    });
                    EventSubscriber.Add(new EventSubscriber()
                    {
                        Id = Guid.NewGuid(),
                        Name = "于中涛",
                        WXName = "YuZhongTao",
                        EMail = "yuzhongtao@jstayc.com",
                        IsEnable = false
                    });
                    EventSubscriber.Add(new EventSubscriber()
                    {
                        Id = Guid.NewGuid(),
                        Name = "董梓莘",
                        WXName = "DongZiShen",
                        EMail = "2795338562@qq.com",
                        IsEnable = false
                    });
                    SaveChanges();
                }

                if (!ClientEventSubscriber.Any())
                {
                    EventSubscriber.Where(r => r.IsEnable).ToList().ToList().ForEach(r =>
                     ClientEventSubscriber.Add(
                     new ClientEventSubscriber()
                     {
                         Id = Guid.NewGuid(),
                         Subscriber = r.Id,
                         Client = "TEST"
                     }));
                    SaveChanges();
                }

                if (!Sender.Any())
                {
                    Sender.Add(new Sender()
                    {
                        Id = Guid.NewGuid(),
                        Name = "同安企业微信",
                        TypeName = Enums.EnumSender.e企业微信.ToString().Substring(1),
                        IsEnable = true,
                        Para = new { agentid = 1000045, url = "http://work.jstayc.com/WechatWebService.asmx" }.SerializeObject()
                    });
                    Sender.Add(new Sender()
                    {
                        Id = Guid.NewGuid(),
                        Name = "中云企业微信",
                        TypeName = Enums.EnumSender.e企业微信.ToString().Substring(1),
                        IsEnable = false,
                        Para = new { agentid = 1000002, url = "http://qywx.bjzycx.net/WechatWebService.asmx" }.SerializeObject()
                    });
                    Sender.Add(new Sender()
                    {
                        Id = Guid.NewGuid(),
                        Name = "126邮箱",
                        TypeName = Enums.EnumSender.e邮箱.ToString().Substring(1),
                        IsEnable = true,
                        Para = new { account = "2739427628@qq.com", password = "dirxzskqwbkwdhbb", smtp = "smtp.qq.com", port = 465, cc = new string[0] }.SerializeObject()
                    });

                    SaveChanges();
                }

                //更新本地标志

            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var c = optionsBuilder.Options.FindExtension<Microsoft.EntityFrameworkCore.Sqlite.Infrastructure.Internal.SqliteOptionsExtension>();
            if (c == null || string.IsNullOrEmpty(c.ConnectionString))
                optionsBuilder.UseSqlite(ConnectionString, opt => opt.MaxBatchSize(1000));// 批处理数量为1000
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            void aa<T>() where T : class
            {
                modelBuilder.Entity<T>().ToTable(typeof(T).Name.ToLower());
            }

            aa<User>();
            aa<ServiceTpl>();
            aa<ServiceInfo>();
            aa<EventInfo>();
            aa<EventSubscriber>();
            aa<EventPushRecord>();
            aa<ClientEventSubscriber>();
            //aa<Card>();
            //aa<Bill>();
            //aa<Tran>();
            //aa<Amortize>();
            //modelBuilder.Entity<Card>().HasOne(b => b.Bank).WithMany().HasForeignKey(b => b.BankId).OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Bill>().HasOne(b => b.Card).WithMany().HasForeignKey(b => b.CardId);
            //modelBuilder.Entity<Tran>().HasOne(b => b.Card).WithMany().HasForeignKey(b => b.CardId);
            modelBuilder.Entity<EventPushRecord>().HasOne(b => b.EventInfo).WithMany().HasForeignKey(b => b.Event);
            modelBuilder.Entity<EventPushRecord>().HasOne(b => b.EventSubscriber).WithMany().HasForeignKey(b => b.Subscriber);
            modelBuilder.Entity<ClientEventSubscriber>().HasOne(b => b.EventSubscriber).WithMany().HasForeignKey(b => b.Subscriber);

        }
        public DbSet<User> User { get; set; }
        public DbSet<ServiceTpl> ServiceTpl { get; set; }
        public DbSet<ServiceInfo> ServiceInfo { get; set; }
        public DbSet<EventInfo> EventInfo { get; set; }
        public DbSet<DogAction> DogAction { get; set; }
        public DbSet<EventSubscriber> EventSubscriber { get; set; }
        public DbSet<EventPushRecord> EventPushRecord { get; set; }
        public DbSet<Sender> Sender { get; set; }
        public DbSet<ClientEventSubscriber> ClientEventSubscriber { get; set; }


    }
}
