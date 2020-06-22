using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Q.DevExtreme.Tpl;
using Q.Helper;
using QServiceDog.Helpers;

namespace QServiceDog
{
    public class Startup : Q.DevExtreme.Tpl.Startup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }
        protected override string CookieName { get; set; } = nameof(QServiceDog);

        public override void ConfigureOther(IApplicationBuilder app, IWebHostEnvironment env)
        {
            new BLL.ServiceDBContext().CreateAndInitData();
            Task.Delay(10000).ContinueWith(t => new QCommon.Service.Jobs.QuartzJobScheduler().RunAsync());
        }

        public override void ConfigureServicesOther(IServiceCollection services)
        {
            Logging.Init(Configuration.GetSection("Logging").Get<Logging>());
            string client = this.Configuration.GetSection("Client").Value;
            if (string.IsNullOrEmpty(client))
                client = nameof(QServiceDog);
            else
                client = client + "." + nameof(QServiceDog);
            LogHelper.Default.StartLogServer(client, Logging.Instance.LogServer, System.Text.Encoding.UTF8);

            //services.AddEntityFrameworkSqlServer();
            //services.AddDbContext<Helpers.LogDBContext>(option => option.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection"), opt => opt.MaxBatchSize(1000)));

#if DEBUG
           
            BLL.ServiceDBContext.ConnectionString =$"Data Source={System.IO.Path.Combine(  "data" , "app.db")};";
#else
            BLL.ServiceDBContext.ConnectionString = $"Data Source={System.IO.Path.Combine(System.IO.Path.Combine(AppContext.BaseDirectory, "data"), "app.db")};";
#endif
            Q.Helper.LogHelper.Default.Info($"ServiceDB={BLL.ServiceDBContext.ConnectionString}");
            services.AddEntityFrameworkSqlite();
            services.AddDbContext<BLL.ServiceDBContext>(options =>
            {
                options.UseSqlite(BLL.ServiceDBContext.ConnectionString, o => o.MinBatchSize(100));
            });

            services.AddScoped<DbContext, BLL.ServiceDBContext>();
            services.AddSingleton<Q.DevExtreme.Tpl.Models.IUserBLL, BLL.UserBLL>();

            Q.DevExtreme.Tpl.Utils.Copyright = this.Configuration.GetSection("AppConfig:Copyright").Value;
            Q.DevExtreme.Tpl.Utils.SUITE_NAME = this.Configuration.GetSection("AppConfig:Name").Value;
            Q.DevExtreme.Tpl.Utils.Web = this.Configuration.GetSection("AppConfig:Web").Value;

            services.AddAutoMapper(typeof(Models.AutoMapping));
            GlobalConfig.Instance.Mapper = new Mapper(new MapperConfiguration(r =>
            {
                r.AddProfile<Models.AutoMapping>();
            }));
            GlobalConfig.Instance.Client = this.Configuration.GetSection("Client").Value;

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        //{
        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }
        //    else
        //    {
        //        app.UseExceptionHandler("/Error");
        //        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        //        app.UseHsts();
        //    }

        //    app.UseHttpsRedirection();
        //    app.UseStaticFiles();

        //    app.UseRouting();

        //    app.UseAuthorization();

        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapRazorPages();
        //    });
        //}
    }
}
