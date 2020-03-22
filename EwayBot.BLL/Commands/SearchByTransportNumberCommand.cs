using EwayBot.DAL.Constants;
using EwayBot.DAL.Services;
using EwayBot.DTO;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace EwayBot.BLL.Commands
{
    public class SearchByTransportNumberCommand : ICommand
    {
        public EwayApiService ewayApiService { get; set; }
        public SearchByTransportNumberCommand(IOptions<SensitiveTokens> sensitiveTokens)
        {
            ewayApiService = new EwayApiService(sensitiveTokens);
        }
        public bool Contains(Message message, string previousMessage = null)
        {
            if (message.Type != MessageType.Text)
                return false;

            return !previousMessage.Contains(Constants.SearchByTransportNumber);
        }

        public async Task Execute(Message message, TelegramBotClient botClient, string previousMessage = null)
        {
            var chatId = message.Chat.Id;
            var routesList = await ewayApiService.GetRoutesList();
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
            var keyboardMarkup = new InlineKeyboardMarkup(Get(listOfTransportNames));

            await botClient.SendTextMessageAsync(chatId, $"Виберіть тип транспорту", parseMode: ParseMode.Markdown, replyMarkup: keyboardMarkup);

        }

        private static InlineKeyboardButton[][] Get(List<string> trpType)
        {
            var index = trpType.Count;
            var res = new InlineKeyboardButton[index][];

            for (int i=0; i<trpType.Count;i++)
            {
                res[i] = new[]
                {
                    new InlineKeyboardButton
                    {
                        Text = trpType[i],
                        CallbackData = "/transportTypeCallback" + trpType[i]
                    }
                };
            }
            return res;
        }
    } 
}
