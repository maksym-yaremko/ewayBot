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

            return fullObject.CallbackQuery.Data.Contains(Constants.StopMapCallback) || fullObject.CallbackQuery.Data.Contains(Constants.LocationMapCallback);
        }

        public async Task Execute(TelegramBotClient botClient, Update fullObject, string previousMessage = null)
        {
            var chatId = fullObject.CallbackQuery.Message.Chat.Id;

            var getStopInfo = await ewayApiService.GetStopInfo(fullObject.CallbackQuery.Message.Venue.Address);

            var getStopInfoNewModel = await ewayApiService.GetStopInfoDestination(fullObject.CallbackQuery.Message.Venue.Address);

            var transportInfo = $"Знайдено маршрутки по зупинці\n";

            foreach (var route in getStopInfoNewModel.routes)
            {
                if (route.transportKey == "marshrutka")
                {
                    if (route.timeLeftFormatted != null)
                    {
                        transportInfo += "🚍 " + "*" + route.title + "*" + "_" +"  (" + route.directionTitle + ") " + "_" + " -> " + "*" + route.timeLeftFormatted + "*" + "\n";
                    }
                }
                if (route.transportKey == "bus")
                {
                    if (route.timeLeftFormatted != null)
                    {
                        transportInfo += "🚌 " + "*" + route.title + "*" + "_" + "  (" + route.directionTitle + ") " + "_" + " -> " + "*"+ route.timeLeftFormatted + "*"+ "\n";
                    }
                }
                if (route.transportKey == "trol")
                {
                    if (route.timeLeftFormatted != null)
                    {
                        transportInfo += "🚎 " + "*" + route.title + "*" + "_" + "  (" + route.directionTitle + ") " + "_" + " -> " + "*"+ route.timeLeftFormatted + "*"+"\n";
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
