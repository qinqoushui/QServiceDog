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
                // Database.EnsureDeleted();
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


                if (!ServiceInfo.Any())
                {
                    ServiceInfo.Add(new ServiceInfo()
                    {
                        Id = Guid.NewGuid(),
                        Desc = "记事本测试",
                        Name = "NotePad",
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
                        IdleTime = TimeSpan.FromMinutes(1),
                        RestartTime = TimeSpan.FromDays(38),
                        Client = "TEST",
                        IsEnable = true
                    });

                    ServiceInfo.Add(new ServiceInfo()
                    {
                        Id = Guid.NewGuid(),
                        Desc = "Url测试",
                        Name = "regedit",
                        CheckName = Enums.EnumAction.e打开网页.ToString().Substring(1),
                        CheckData = "http://192.168.10.37:8080",
                        RunName = Enums.EnumAction.e启动进程.ToString().Substring(1),
                        RunData = new
                        {
                            FileName = @"c:\windows\system32\regedit.exe"
                        }.SerializeObject(),
                        StopName = Enums.EnumAction.e终止进程.ToString().Substring(1),
                        StopData = "regedit",
                        LastAliveTime = DateTime.Now,
                        LastStopTime = DateTime.Now,
                        IdleTime = TimeSpan.FromMinutes(1),
                        RestartTime = TimeSpan.FromMinutes(3),
                        Client = "TEST",
                        IsEnable = true
                    });

                    ServiceInfo.Add(new ServiceInfo()
                    {
                        Id = Guid.NewGuid(),
                        Desc = "SQLServer数据库",
                        Name = "SQLServer",
                        CheckName = Enums.EnumAction.e检测端口.ToString().Substring(1),
                        CheckData = "127.0.0.1:1433",
                        RunName = Enums.EnumAction.e启动服务.ToString().Substring(1),
                        RunData = @"MSSQL$SQL2014",
                        StopName = Enums.EnumAction.e停止服务.ToString().Substring(1),
                        StopData = "MSSQL$SQL2014",
                        LastAliveTime = DateTime.Now,
                        LastStopTime = DateTime.Now,
                        IdleTime = TimeSpan.FromMinutes(1),
                        RestartTime = TimeSpan.FromMinutes(3),
                        Client = "TEST",
                        IsEnable = true
                    });

                    ServiceInfo.Add(new ServiceInfo()
                    {
                        Id = Guid.NewGuid(),
                        Desc = "Redis数据库",
                        Name = "Redis",
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
                        Client = "TEST",
                        IsEnable = true
                    });

                    ServiceInfo.Add(new ServiceInfo()
                    {
                        Id = Guid.NewGuid(),
                        Desc = "OpenVPN守护",
                        Name = "OpenVPN",
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
                        Client = "TEST",
                        IsEnable = true
                    });
                    SaveChanges();
                }
                else
                {
                    if (!clientName.Equals("CLOUD", StringComparison.OrdinalIgnoreCase))
                    {
                        ServiceInfo.Where(r => r.Client != clientName).ToList().ForEach(r => r.Client = clientName);
                        SaveChanges();
                    }
                }

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
        public DbSet<ServiceInfo> ServiceInfo { get; set; }
        public DbSet<EventInfo> EventInfo { get; set; }
        public DbSet<DogAction> DogAction { get; set; }
        public DbSet<EventSubscriber> EventSubscriber { get; set; }
        public DbSet<EventPushRecord> EventPushRecord { get; set; }
        public DbSet<Sender> Sender { get; set; }
        public DbSet<ClientEventSubscriber> ClientEventSubscriber { get; set; }


    }
}
