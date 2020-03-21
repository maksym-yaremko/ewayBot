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
    public class PreviousTransportNameCallback : ICallback
    {
        public EwayApiService ewayApiService { get; set; }
        public PreviousTransportNameCallback(IOptions<SensitiveTokens> sensitiveTokens)
        {
            ewayApiService = new EwayApiService(sensitiveTokens);
        }
        public bool Contains(Update fullObject, string previousMessage = null)
        {
            var callBackMessage = fullObject.CallbackQuery.Data;

            return callBackMessage.Contains(Constants.PreviousTransportNameCallbackButton);
        }

        public async Task Execute(TelegramBotClient botClient, Update fullObject, string previousMessage = null)
        {
            var chatId = fullObject.CallbackQuery.Message.Chat.Id;
            var callBackMessage = fullObject.CallbackQuery.Data;

            var stringSplitByNextTransportNameCallbackButton = callBackMessage.Split(new string[] { Constants.PreviousTransportNameCallbackButton }, StringSplitOptions.None);

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

            await botClient.EditMessageTextAsync(chatId,fullObject.CallbackQuery.Message.MessageId, $"Виберіть номер" + transportTypeTranslated, replyMarkup: keyboardMarkup);
        }


        private static InlineKeyboardButton[][] Get(List<string> trpName, Update fullObject, string transportType, string indexOfTransportInList)
        {
            var index = int.Parse(indexOfTransportInList);

            if (index % 10!=0)
            {
                var dividing = index % 10;
                var finalSendIndex = index - dividing;
                var finalIndex = index - dividing - 10;


                var res = new InlineKeyboardButton[11][];

                for (int i = 0; i < 11; i++)
                {
                    if (i % 10 != 0 || i == 0)
                    {
                        res[i] = new[]
                        {
                        new InlineKeyboardButton
                        {
                            Text = trpName[finalIndex],
                            CallbackData = "/transportNameCallback" + trpName[finalIndex]
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
                                CallbackData = finalSendIndex + "/previousTransportNameCallbackButton" + transportType + "/editedMessagedId" + fullObject.CallbackQuery.Message.MessageId
                            },
                            new InlineKeyboardButton
                            {
                                Text = "Next",
                                CallbackData = finalSendIndex + "/nextTransportNameCallbackButton" + transportType + "/editedMessagedId" + fullObject.CallbackQuery.Message.MessageId
                            }
                        };
                    }
                    finalIndex++;
                }

                return res;
            }

            else if (index == 20)
            {
                var resul = new InlineKeyboardButton[11][];
                var indexSendToCallback = index - 10;
                index = index - 20;

                for (int i = 0; i < 11; i++)
                {
                    if (i % 10 != 0 || i == 0)
                    {
                        resul[i] = new[]
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
                        resul[i] = new[]
                        {
                            new InlineKeyboardButton
                            {
                                Text = "Next",
                                CallbackData = indexSendToCallback + "/nextTransportNameCallbackButton" + transportType + "/editedMessagedId" + fullObject.CallbackQuery.Message.MessageId
                            }
                        };
                    }
                    index++;
                }
                return resul;
            }

            var result = new InlineKeyboardButton[11][];
            var indexSend = index-10;
            index = index - 20;

            for (int i = 0; i < 11; i++)
            {
                if (i % 10 != 0 || i == 0)
                {
                    result[i] = new[]
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
                    result[i] = new[]
                    {
                            new InlineKeyboardButton
                            {
                                Text = "Previous",
                                CallbackData = indexSend + "/previousTransportNameCallbackButton" + transportType + "/editedMessagedId" + fullObject.CallbackQuery.Message.MessageId
                            },
                            new InlineKeyboardButton
                            {
                                Text = "Next",
                                CallbackData = indexSend + "/nextTransportNameCallbackButton" + transportType + "/editedMessagedId" + fullObject.CallbackQuery.Message.MessageId
                            }
                        };
                }
                index++;
            }

            return result;
        }
    }
}
