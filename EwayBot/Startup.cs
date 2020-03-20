using EwayBot.BLL;
using EwayBot.DAL.Context;
using EwayBot.DAL.Services;
using EwayBot.DTO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

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
            services.Configure<SensitiveTokens>(Configuration.GetSection("SensitiveTokens"));
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

            TelegramBotClient botClient = new TelegramBotClient("1096673257:AAGq_sGCLZ2z-g6IPdtsTuwTcfj1qt0DfGM");
            string hook = string.Format("https://065ff8c4.ngrok.io/api/message/update");
            botClient.SetWebhookAsync(hook);
        }
    }
}
