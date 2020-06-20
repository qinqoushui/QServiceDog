﻿using Microsoft.EntityFrameworkCore;
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
                        Command = "sc stop",
                        Name = Enums.EnumAction.e停止服务.ToString().Substring(1),
                        Type = "Stop"
                    });

                    DogAction.Add(new DogAction()
                    {
                        Id = Guid.NewGuid(),
                        Command = "sc start",
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
                        CheckName = "查找进程",
                        CheckData = "Notepad",
                        RunName = "启动进程",
                        RunData = @"c:\windows\system32\notepad.exe",
                        StopName = "终止进程",
                        StopData = "notepad",
                        LastAliveTime = DateTime.Now,
                        LastStopTime = DateTime.Now,
                        IdleTime = TimeSpan.FromMinutes(1),
                        RestartTime = TimeSpan.FromDays(38),
                        IsEnable = true
                    });

                    ServiceInfo.Add(new ServiceInfo()
                    {
                        Id = Guid.NewGuid(),
                        Desc = "Url测试",
                        Name = "calc",
                        CheckName = "打开网页",
                        CheckData = "192.168.10.37:8080",
                        RunName = "启动进程",
                        RunData = @"c:\windows\system32\calc.exe",
                        StopName = "终止进程",
                        StopData = "calc",
                        LastAliveTime = DateTime.Now,
                        LastStopTime = DateTime.Now,
                        IdleTime = TimeSpan.FromMinutes(1),
                        RestartTime = TimeSpan.FromMinutes(3),
                        IsEnable = true
                    });

                    ServiceInfo.Add(new ServiceInfo()
                    {
                        Id = Guid.NewGuid(),
                        Desc = "Redis数据库",
                        Name = "Redis",
                        CheckName = "检测端口",
                        CheckData = "127.0.0.1:6379",
                        RunName = "启动服务",
                        RunData = @"redis",
                        StopName = "停止服务",
                        StopData = "redis",
                        LastAliveTime = DateTime.Now,
                        LastStopTime = DateTime.Now,
                        IdleTime = TimeSpan.FromMinutes(1),
                        RestartTime = TimeSpan.FromMinutes(3),
                        IsEnable = true
                    });

                    ServiceInfo.Add(new ServiceInfo()
                    {
                        Id = Guid.NewGuid(),
                        Desc = "OpenVPN守护",
                        Name = "OpenVPN",
                        CheckName = "检测IP",
                        CheckData = "192.168.255.1",
                        RunName = "启动进程",
                        RunData = new
                        {
                            FileName = @"C:\Program Files\OpenVPN\bin\openvpn.exe",
                            Para = @"""d:\data\openvpn\config\aliyun.ovpn""",
                            WorkingPath = @"d:\data\openvpn\config"
                        }.SerializeObject(),
                        StopName = "终止进程",
                        StopData = "openvpn*",
                        LastAliveTime = DateTime.Now,
                        LastStopTime = DateTime.Now,
                        IdleTime = TimeSpan.FromMinutes(1),
                        RestartTime = TimeSpan.FromMinutes(5),
                        IsEnable = true
                    });
                    SaveChanges();
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
        public DbSet<DogAction> DogAction { get; set; }



    }
}
