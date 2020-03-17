using EwayBot.DAL.Constants;
using EwayBot.DAL.Services;
using EwayBot.DTO;
using GeoCoordinatePortable;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EwayBot.BLL.Buttons
{
    public class LocationButton : IButton
    {
        public EwayApiService ewayApiService { get; set; }
        public LocationButton(IOptions<SensitiveTokens> sensitiveTokens)
        {
            ewayApiService = new EwayApiService(sensitiveTokens);
        }
        public bool Contains(Message message, string previousMessage = null)
        {
            return previousMessage.Contains(Constants.SearchStopByYourLocation);
        }

        public async Task Execute(Message message, TelegramBotClient botClient, string previousMessage = null)
        {
            var chatId = message.Chat.Id;
            var result = await ewayApiService.GetStopsNearPoint(message.Location.Latitude.ToString(CultureInfo.InvariantCulture),message.Location.Longitude.ToString(CultureInfo.InvariantCulture));
            var messageRegardingStopsByLocation = "Знайдено зупинки в районі 300 метрів\n"; 
            foreach(var res in result.stop)
            {
                var sCoord = new GeoCoordinate(message.Location.Latitude, message.Location.Longitude);
                var eCoord = new GeoCoordinate(double.Parse(res.Lat, CultureInfo.InvariantCulture.NumberFormat), double.Parse(res.Lng, CultureInfo.InvariantCulture.NumberFormat));

                var metres = sCoord.GetDistanceTo(eCoord);

                messageRegardingStopsByLocation += "🚏" +res.Title +"-"+ (int)metres + "\n";
            }
            if (result.stop.Count == 0)
            {
                messageRegardingStopsByLocation = "Не знайдено зупинок в районі 300 метрів";
            }
            await botClient.SendTextMessageAsync(chatId, messageRegardingStopsByLocation, parseMode: ParseMode.Markdown);
        }
    }
}
