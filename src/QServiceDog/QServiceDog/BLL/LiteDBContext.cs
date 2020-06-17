using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Q.DevExtreme.Tpl.Models;
using Q.DevExtreme.Tpl;
using System;
using System.Collections.Generic;
using System.Linq;

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
            //            if (!_created)
            //            {
            //                _created = true;
            //#if DEBUG
            //                Database.EnsureDeleted();
            //#endif
            //                Database.EnsureCreated(); //无法打开数据库，原来是要Microsoft.EntityFrameworkCore.Sqlite     
            //            }
            //            Initdata();
        }



        public void CreateAndInitData()
        {
            Database.EnsureCreated();
            if (Database.EnsureCreated())
            {
                if (!User.Any())
                {
                    User.Add(new Q.DevExtreme.Tpl.Models.User()
                    {
                        Id = Guid.NewGuid(),
                        UserNo = "admin",
                        Password = Extension.makePassword("admin"),
                        UserName = "admin",
                        Role = "管理员"
                    });
                    SaveChanges();
                }
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var c = optionsBuilder.Options.FindExtension<Microsoft.EntityFrameworkCore.Sqlite.Infrastructure.Internal.SqliteOptionsExtension>();
            if (c == null || string.IsNullOrEmpty(c.ConnectionString))
                optionsBuilder.UseSqlServer(ConnectionString, opt => opt.MaxBatchSize(1000));// 批处理数量为1000
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            void aa<T>() where T : class
            {
                modelBuilder.Entity<T>().ToTable(typeof(T).Name.ToLower());
            }

            aa<User>();
            //aa<Owner>();
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



    }
}
