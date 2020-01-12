using System;
using EwayBot.DAL.Context;
using EwayBot.DAL.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace EwayBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Eway bot has been started");
            try
            {
                var configuration = ReadConfiguration(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));

                Serilog.Debugging.SelfLog.Enable(Console.Error);

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();

                Log.Information("Starting up");

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                    .UseSerilog()
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });


        private static IConfiguration ReadConfiguration(string environment)
        {
            var config = new ConfigurationBuilder();

            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

            return config.Build();
        }
    }
}
