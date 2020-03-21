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
using Telegram.Bot.Types.ReplyMarkups;

namespace EwayBot.BLL.Callbacks
{
    public class NextTransportNameCallback : ICallback
    {
        public EwayApiService ewayApiService { get; set; }
        public NextTransportNameCallback(IOptions<SensitiveTokens> sensitiveTokens)
        {
            ewayApiService = new EwayApiService(sensitiveTokens);
        }
        public bool Contains(Update fullObject, string previousMessage = null)
        {
            var callBackMessage = fullObject.CallbackQuery.Data;

            return callBackMessage.Contains(Constants.NextTransportNameCallbackButton);
        }

        public async Task Execute(TelegramBotClient botClient, Update fullObject, string previousMessage = null)
        {
            var chatId = fullObject.CallbackQuery.Message.Chat.Id;
            var callBackMessage = fullObject.CallbackQuery.Data;

            var stringSplitByNextTransportNameCallbackButton = callBackMessage.Split(new string[] { Constants.NextTransportNameCallbackButton }, StringSplitOptions.None);

            var indexOfTransportInList = stringSplitByNextTransportNameCallbackButton[0];

            var stringSplitByEditedMessagedId = stringSplitByNextTransportNameCallbackButton[1].Split(new string[] { Constants.EditedMessagedId }, StringSplitOptions.None);

            var transportType = stringSplitByEditedMessagedId[0];
            var messageId = stringSplitByEditedMessagedId[1];


            var transportTypeTranslated = "";
            if (transportType == "bus")
            {
                transportTypeTranslated = "🚌 маршрутки";
            }
            else if (transportType == "trol")
            {
                transportTypeTranslated = "🚎 тролейбуса";
            }
            else if (transportType == "tram")
            {
                transportTypeTranslated = "🚊 трамвая";
            }

            var routesList = await ewayApiService.GetRoutesList();

            var routesListByTransport = routesList.routesList.route.Where(x => x.Transport == transportType);

            var listOfTransportNames = new List<string>();

            foreach (var route in routesListByTransport)
            {
                listOfTransportNames.Add(route.Title);
            }


            var keyboardMarkup = new InlineKeyboardMarkup(Get(listOfTransportNames, fullObject, transportType, indexOfTransportInList));

            await botClient.EditMessageTextAsync(chatId,/*int.Parse(messageId)*/fullObject.CallbackQuery.Message.MessageId, $"Виберіть номер" + transportTypeTranslated, replyMarkup: keyboardMarkup);
        }


        private static InlineKeyboardButton[][] Get(List<string> trpName, Update fullObject, string transportType,string indexOfTransportInList)
        {
            var index = int.Parse(indexOfTransportInList);
            var newIndex = index;

            if(trpName.Count - index < 10)
            {
                var newRes = new InlineKeyboardButton[trpName.Count-newIndex + 1][];
                for(int i = 0; i < trpName.Count - newIndex + 1; i++)
                {
                    if (i != trpName.Count - newIndex)
                    {
                        newRes[i] = new[]
                        {
                        new InlineKeyboardButton
                        {
                            Text = trpName[index],
                            CallbackData = "/transportNameCallback" + trpName[index]
                        }
                    };
                    }
                    else
                    {
                        newRes[i] = new[]
                        {
                            new InlineKeyboardButton
                            {
                                Text = "Previous",
                                CallbackData = index + "/previousTransportNameCallbackButton" + transportType + "/editedMessagedId" + fullObject.CallbackQuery.Message.MessageId
                            }
                        };
                    }
                    index++;
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
                            Text = trpName[index],
                            CallbackData = "/transportNameCallback" + trpName[index]
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
                                CallbackData = index + "/previousTransportNameCallbackButton" + transportType + "/editedMessagedId" + fullObject.CallbackQuery.Message.MessageId
                            },
                            new InlineKeyboardButton
                            {
                                Text = "Next",
                                CallbackData = index + "/nextTransportNameCallbackButton" + transportType + "/editedMessagedId" + fullObject.CallbackQuery.Message.MessageId
                            }
                        };
                }
                index++;
            }
            return res;
        }
    }
}
