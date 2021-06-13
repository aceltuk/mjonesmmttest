using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MJonesMmtTest.Api.Managers;
using MJonesMmtTest.Api.Models.Config;
using MJonesMmtTest.Api.Models.Data;
using MJonesMmtTest.Api.Repositories;
using MJonesMmtTest.Api.Services;

namespace MJonesMmtTest.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<SseTestContext>(opts =>
            {
                opts.UseSqlServer(Configuration["OrderDbConnection"]);
            });

            //inject our dependencies here
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IOrderHistoryManager, OrderHistoryManager>();

            //no need to alter the config data once loaded
            var config = new AppConfiguration { ApiKey = Configuration["ApiKey"], ApiEndpoint = Configuration["ApiEndpoint"] };
            services.AddSingleton(config);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            // simple logging introduction
            loggerFactory.AddFile("Logs/mylog-{Date}.txt");

            app.UseStatusCodePages();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
