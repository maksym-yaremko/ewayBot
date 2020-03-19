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
using Telegram.Bot.Types.ReplyMarkups;

namespace EwayBot.BLL.Buttons
{
    public class LocationButton : IButton
    {
        public EwayApiService ewayApiService { get; set; }
        public StopService stopService { get; set; }
        public LocationButton(IOptions<SensitiveTokens> sensitiveTokens)
        {
            ewayApiService = new EwayApiService(sensitiveTokens);
            stopService = new StopService();
        }
        public bool Contains(Message message, string previousMessage = null)
        {
            return previousMessage.Contains(Constants.SearchStopByYourLocation);
        }

        public async Task Execute(Message message, TelegramBotClient botClient, string previousMessage = null)
        {
            var buttonItem = new List<string>();
            
            var chatId = message.Chat.Id;
            var result = await ewayApiService.GetStopsNearPoint(message.Location.Latitude.ToString(CultureInfo.InvariantCulture),message.Location.Longitude.ToString(CultureInfo.InvariantCulture));
            var messageRegardingStopsByLocation = "Найближчі зупинки ⤵️"; 
            foreach(var res in result.stop)
            {
                var sCoord = new GeoCoordinate(message.Location.Latitude, message.Location.Longitude);
                var eCoord = new GeoCoordinate(double.Parse(res.Lat, CultureInfo.InvariantCulture.NumberFormat), double.Parse(res.Lng, CultureInfo.InvariantCulture.NumberFormat));

                var metres = sCoord.GetDistanceTo(eCoord);

                buttonItem.Add("🚏 " + res.Title +" - "+ (int)metres + " m");
            }
            if (result.stop.Count == 0)
            {
                messageRegardingStopsByLocation = "Не знайдено зупинок поблизу вас";
            }

            //var keyboardMarkup = new InlineKeyboardMarkup(GetInlineKeyboard(buttonItem));
            await botClient.SendTextMessageAsync(chatId, messageRegardingStopsByLocation, parseMode: ParseMode.Markdown);

            foreach (var stop in result.stop)
            {
                var stopDb = stopService.GetStopByLocation(stop.Lat,stop.Lng);
                await botClient.SendVenueAsync(chatId, float.Parse(stop.Lat, CultureInfo.InvariantCulture.NumberFormat), float.Parse(stop.Lng, CultureInfo.InvariantCulture.NumberFormat), stop.Title, stopDb.Id, replyMarkup: GetInlineKeyboard());
            }
        }


        private static InlineKeyboardMarkup GetInlineKeyboard()
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Обрати", "/locationMapCallback"),
                        }
                    });
            return inlineKeyboard;
        }
    }
}
