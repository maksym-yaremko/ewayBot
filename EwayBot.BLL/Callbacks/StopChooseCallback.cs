using EwayBot.DAL.Constants;
using EwayBot.DAL.Services;
using EwayBot.DTO;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EwayBot.BLL.Callbacks
{
    public class StopChooseCallback : ICallback
    {
        public EwayApiService ewayApiService { get; set; }
        public StopChooseCallback(IOptions<SensitiveTokens> sensitiveTokens)
        {
            ewayApiService = new EwayApiService(sensitiveTokens);
        }
        public bool Contains(Update fullObject, string previousMessage = null)
        {
            //if (fullObject.Message.Type != MessageType.Text)
            //    return false;

            return fullObject.CallbackQuery.Data.Contains(Constants.StopMapCallback);
        }

        public async Task Execute(TelegramBotClient botClient, Update fullObject, string previousMessage = null)
        {
            var chatId = fullObject.CallbackQuery.Message.Chat.Id;

            var getStopInfo = await ewayApiService.GetStopInfo(fullObject.CallbackQuery.Message.Venue.Address);

            var transportInfo = $"Знайдено маршрутки по зупинці\n";

            foreach (var stop in getStopInfo.stop)
            {
                foreach (var trans in stop.Transports.transport)
                {
                    foreach (var route in trans.Route)
                    {
                        if (trans.Attributes.Key == "marshrutka")
                        {
                            transportInfo += "🚍" + route.Title + "->" + route.Next_Vehicle + "\n";
                        }
                        if (trans.Attributes.Key == "bus")
                        {
                            transportInfo += "🚌" + route.Title + "->" + route.Next_Vehicle  +"\n";
                        }
                        if (trans.Attributes.Key == "trol")
                        {
                            transportInfo += "🚎" + route.Title + "->" + route.Next_Vehicle + "\n";
                        }
                    }
                }


            }
            if (getStopInfo.stop.Count == 0)
            {
                transportInfo = "Не знайдено зупинок в районі 300 метрів";
            }

            await botClient.SendTextMessageAsync(chatId, transportInfo, parseMode: ParseMode.Markdown);
            
        }
    }
}
