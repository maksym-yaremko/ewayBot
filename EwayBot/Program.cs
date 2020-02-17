using System;
using System.Globalization;
using System.Linq;
using EwayBot.BLL.EwayAPI;
using EwayBot.DAL.Context;
using EwayBot.DAL.Seeders;
using EwayBot.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace EwayBot
{
    public class Program
    {
        private static TelegramBotClient bot;
        private static ApplicationContext _context;
        private static EwayAPIClient _client;
        public static void Main(string[] args)
        {
            Console.WriteLine("Eway bot has been started");
            try
            {
                var builder = CreateHostBuilder(args).Build();
                var scope1 = builder.Services.CreateScope();
                var services1 = scope1.ServiceProvider; 
                var settings = services1.GetService<IOptions<TelegramSettings>>();
                var ewaySettings = services1.GetService<IOptions<EwayAPISettings>>();
                _context = services1.GetService<ApplicationContext>();
                _client = new EwayAPIClient(ewaySettings);
                bot = new TelegramBotClient(settings.Value.APIToken);
                bot.OnMessage += MessageHandler;
                bot.OnCallbackQuery += BotOnCallbackQueryReceived;
                bot.StartReceiving();

                var configuration = ReadConfiguration(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));

                Serilog.Debugging.SelfLog.Enable(Console.Error);

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();

                Log.Information("Starting up");

                using (var scope = builder.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var context = services.GetService<ApplicationContext>();

                    StopsSeeder seeder = new StopsSeeder();
                    if(!context.Stops.Any())
                        seeder.InitializeStops(context);
                }


                builder.Run();
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

        private static async void MessageHandler(object sender, MessageEventArgs e)
        {
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                PrepareQuestionnaires(e);
            else
            {
                await bot.SendTextMessageAsync(e.Message.Chat.Id, "������ ����� �������!");
            }
        }
        public static async void PrepareQuestionnaires(MessageEventArgs e)
        {
            if (e.Message.Text.ToLower().Contains("start"))
            {
                await bot.SendTextMessageAsync(e.Message.Chat.Id, "³���� ��� � LvivPublicTransportBot.��� �������� ������-����� ������ ������ ����� ������� � ������� �� �� ���");
                return;
            }
            var stops = await _context.Stops.AsNoTracking().Where(x => x.Title.ToLower().Contains(e.Message.Text.ToLower())).ToListAsync();
            if (stops.Count == 0)
            {
                await bot.SendTextMessageAsync(e.Message.Chat.Id, "������� � ����� ������ ����!");
                return;
            }

            foreach (var stop in stops)
            {
                var inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(stop.Title, stop.Id)
                    }
                });
                await bot.SendLocationAsync(e.Message.Chat.Id, float.Parse(stop.Lat, CultureInfo.InvariantCulture.NumberFormat), float.Parse(stop.Lng, CultureInfo.InvariantCulture.NumberFormat), replyMarkup: inlineKeyboard);
            }
        }

        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;
            var stopId = callbackQuery.Data;

            var stopInfo = await _client.GetStopInfo(Int32.Parse(stopId));
            var transports = stopInfo.stop.First().Transports.Transport;
            foreach (var transport in transports)
            {
                foreach (var route in transport.Route)
                {
                    if (route.Next_Vehicle.HasValue && route.Has_Gps == "1")
                    {
                        await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id,
                            $"{transport.Attributes.Name} {route.Title} ������� ����� {route.Next_Vehicle} ������.");
                    }
                    else
                    {
                        await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id,
                            $"���� ���������� ���:{transport.Attributes.Name} {route.Title}");
                    }
                }
            }
        }
    }
}
