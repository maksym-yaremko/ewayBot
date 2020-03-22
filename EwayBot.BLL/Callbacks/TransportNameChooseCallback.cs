using EwayBot.DAL.Constants;
using EwayBot.DAL.Services;
using EwayBot.DTO;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EwayBot.BLL.Callbacks
{
    public class TransportNameChooseCallback : ICallback
    {
        public EwayApiService ewayApiService { get; set; }
        public TransportNameChooseCallback(IOptions<SensitiveTokens> sensitiveTokens)
        {
            ewayApiService = new EwayApiService(sensitiveTokens);
        }
        public bool Contains(Update fullObject, string previousMessage = null)
        {
            var callBackMessage = fullObject.CallbackQuery.Data;

            return callBackMessage.Contains(Constants.TransportNameCallback);
        }

        public async Task Execute(TelegramBotClient botClient, Update fullObject, string previousMessage = null)
        {
            var chatId = fullObject.CallbackQuery.Message.Chat.Id;
            var callBackMessage = fullObject.CallbackQuery.Data;

            var stringSplitByTransportNameCallback = callBackMessage.Split(new string[] { Constants.TransportNameCallback }, StringSplitOptions.None);

            var transportType = stringSplitByTransportNameCallback[0];
            var transportName = stringSplitByTransportNameCallback[1];

            var routesList = await ewayApiService.GetRoutesList();

            var routesListByTransport = routesList.routesList.route.Where(x => x.Transport == transportType);

            var routeByTransportName = routesListByTransport.SingleOrDefault(x => x.Title == transportName);

            var getRouteInfo = await ewayApiService.GetRouteInfo(routeByTransportName.Id);

            var transportTypeTranslated = "";
            if (transportType == "bus")
            {
                transportTypeTranslated = "🚌 маршрутка";
            }
            else if (transportType == "trol")
            {
                transportTypeTranslated = "🚎 тролейбус";
            }
            else if (transportType == "tram")
            {
                transportTypeTranslated = "🚊 трамвай";
            }

            await botClient.SendTextMessageAsync(chatId, @$"Повна інформація про маршрут " + "*" + transportName + "*" + ":\n" +
                "*Тип транспорту : *" + transportTypeTranslated +"\n" +
                "*Ціна : *" + getRouteInfo.route.Price + " грн " + "\n" +
                "*Інтервал руху : *" + getRouteInfo.route.Interval + " хв" + "\n" +
                "*Робочі години : *" + getRouteInfo.route.Work_Time+ "\n"+
                "*Опис : *" + getRouteInfo.route.Short_Description +"\n" +
                "*Повний маршрут : *" + getRouteInfo.route.Description, parseMode: ParseMode.Markdown);

        }
    }
}
