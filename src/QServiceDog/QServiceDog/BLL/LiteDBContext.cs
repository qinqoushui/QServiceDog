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

        public void CreateAndInitData()
        {
            lock (lck)
            {
                if (_created)
                    return;
                _created = true;
#if DEBUG
                Database.EnsureDeleted();
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
                if (!ServiceInfo.Any())
                {
                    ServiceInfo.Add(new ServiceInfo()
                    {
                        Id = Guid.NewGuid(),
                        Check = "GetProcess Notepad",
                        Desc = "记事本测试",
                        Name = "NotePad",
                        RestartTime = TimeSpan.FromMinutes(3),
                        AliveTime = DateTime.Now,
                        Run = @"c:\windows\system32\notepad.exe",
                        Stop = "KillProcess notepad",
                        IdleTime = TimeSpan.FromMinutes(1),
                    });

                    ServiceInfo.Add(new ServiceInfo()
                    {
                        Id = Guid.NewGuid(),
                        Check = "Get 192.168.10.37:8080",
                        Desc = "Url测试",
                        Name = "calc",
                        RestartTime = TimeSpan.FromMinutes(3),
                        AliveTime = DateTime.Now,
                        Run = @"c:\windows\system32\calc.exe",
                        Stop = "KillProcess calc",
                        IdleTime = TimeSpan.FromMinutes(1),
                    });

                    ServiceInfo.Add(new ServiceInfo()
                    {
                        Id = Guid.NewGuid(),
                        Check = "Telnet 127.0.0.1:6379",
                        Run = @"sc start redis",
                        Stop = "sc stop redis",
                        Desc = "Redis数据库",
                        Name = "Redis",
                        RestartTime = TimeSpan.FromMinutes(3),
                        AliveTime = DateTime.Now,
                        IdleTime = TimeSpan.FromMinutes(1),
                    });

                    ServiceInfo.Add(new ServiceInfo()
                    {
                        Id = Guid.NewGuid(),
                        Check = "ping 192.168.255.1",
                        Run = @"C:\Program Files\OpenVPN\bin\openvpn.exe ""d:\data\openvpn\config\aliyun.ovpn""",
                        Stop = "KillProcess openvpn*",
                        Desc = "OpenVPN守护",
                        Name = "OpenVPN",
                        RestartTime = TimeSpan.FromMinutes(3),
                        AliveTime = DateTime.Now,
                        IdleTime = TimeSpan.FromMinutes(1),
                    });
                }
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
            //aa<MailAccount2>();
            //aa<Bank>();
            //aa<Card>();
            //aa<Bill>();
            //aa<Tran>();
            //aa<Amortize>();
            //modelBuilder.Entity<Card>().HasOne(b => b.Bank).WithMany().HasForeignKey(b => b.BankId).OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Bill>().HasOne(b => b.Card).WithMany().HasForeignKey(b => b.CardId);
            //modelBuilder.Entity<Tran>().HasOne(b => b.Card).WithMany().HasForeignKey(b => b.CardId);
            //modelBuilder.Entity<Amortize>().HasOne(b => b.Card).WithMany().HasForeignKey(b => b.CardId);

        }
        public DbSet<User> User { get; set; }
        public DbSet<ServiceInfo> ServiceInfo { get; set; }
        public DbSet<EventInfo> EventInfo { get; set; }



    }
}
