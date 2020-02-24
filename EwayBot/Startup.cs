using EwayBot.BLL.EwayAPI;
using EwayBot.BLL.Telegram;
using EwayBot.DAL.Context;
using EwayBot.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EwayBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
                     options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection")));
            services.Configure<EwayAPISettings>(options => Configuration.GetSection("Eway").Bind(options));
            services.Configure<TelegramSettings>(options => Configuration.GetSection("Telegram").Bind(options));
            services.AddScoped<TelegramBotService>();
            services.AddScoped<EwayAPIClient>();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
