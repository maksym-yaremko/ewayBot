using EwayBot.DAL.Constants;
using EwayBot.DAL.Services;
using EwayBot.DTO;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace EwayBot.BLL.Callbacks
{
    public class TtransportTypeChooseCallback : ICallback
    {
        public EwayApiService ewayApiService { get; set; }
        public TtransportTypeChooseCallback(IOptions<SensitiveTokens> sensitiveTokens)
        {
            ewayApiService = new EwayApiService(sensitiveTokens);
        }
        public bool Contains(Update fullObject, string previousMessage = null)
        {
            bool IsCallbackValid = false;
            var routesList = ewayApiService.GetRoutesList().Result;
            var listOfTransportNames = new List<string>();
            foreach (var route in routesList.routesList.route)
            {

                if (listOfTransportNames.Contains(route.Transport))
                {
                    continue;
                }
                else
                {
                    listOfTransportNames.Add(route.Transport);
                }
            }

            IsCallbackValid = listOfTransportNames.Any(name => fullObject.CallbackQuery.Data.Contains(Constants.TransportTypeCallback + name));

            return IsCallbackValid;
        }

        public async Task Execute(TelegramBotClient botClient, Update fullObject, string previousMessage = null)
        {
            var chatId = fullObject.CallbackQuery.Message.Chat.Id;
            var transportTypeTranslated = "";

            var transportType = fullObject.CallbackQuery.Data.Remove(fullObject.CallbackQuery.Data.IndexOf(Constants.TransportTypeCallback), Constants.TransportTypeCallback.Length);

            var routesList = await ewayApiService.GetRoutesList();

            var routesListByTransport = routesList.routesList.route.Where(x => x.Transport == transportType);

            var listOfTransportNames = new List<string>();

            foreach(var route in routesListByTransport)
            {
                listOfTransportNames.Add(route.Title);
            }

            if (transportType=="bus")
            {
                transportTypeTranslated = "🚌 маршрутки";
            }
            else if (transportType=="trol")
            {
                transportTypeTranslated = "🚎 тролейбуса";
            }
            else if(transportType == "tram")
            {
                transportTypeTranslated = "🚊 трамвая";
            }

            var keyboardMarkup = new InlineKeyboardMarkup(Get(listOfTransportNames,fullObject, transportType));


            await botClient.SendTextMessageAsync(chatId, $"Виберіть номер" + transportTypeTranslated, parseMode: ParseMode.Markdown, replyMarkup: keyboardMarkup);
        }


        private static InlineKeyboardButton[][] Get(List<string> trpName, Update fullObject, string transportType)
        {
            if (trpName.Count < 10)
            {
                var newRes = new InlineKeyboardButton[trpName.Count][];
                for (int i = 0; i < trpName.Count; i++)
                {
                    newRes[i] = new[]
                    {
                        new InlineKeyboardButton
                        {
                            Text = trpName[i],
                            CallbackData = "/transportNameCallback" + trpName[i]
                        }
                    };
                }
                return newRes;
            }

            var res = new InlineKeyboardButton[11][];

            for (int i = 0; i < 11; i++)
            {
                if (i % 10 != 0 || i == 0)
                {
                    res[i] = new[]
                    {
                    new InlineKeyboardButton
                    {
                        Text = trpName[i],
                        CallbackData = "/transportNameCallback" + trpName[i]
                    }
                };
                }
                else
                {
                    res[i] = new[]
                    {
                        new InlineKeyboardButton
                        {
                            Text = "Previous",
                            CallbackData = 10 + "/previousTransportNameCallbackButton" + transportType + "/editedMessagedId"+ fullObject.CallbackQuery.Message.MessageId
                        },
                        new InlineKeyboardButton
                        {
                            Text = "Next",
                            CallbackData = 10 + "/nextTransportNameCallbackButton" + transportType + "/editedMessagedId"+ fullObject.CallbackQuery.Message.MessageId
                        }
                    };
                }
            }
            return res;
        }
    }
}
