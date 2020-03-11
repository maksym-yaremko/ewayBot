using EwayBot.DAL.Constants;
using EwayBot.DAL.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EwayBot.BLL.Commands
{
    public class SimpleTextCommand : ICommand
    {
        public UserMessageService userMessageService { get; set; }
        public SimpleTextCommand()
        {
            userMessageService = new UserMessageService();
        }
        public bool Contains(Message message, string previousMessage)
        {
            if (message.Type != MessageType.Text)
                return false;

            return previousMessage.Contains(Constants.SearchByStopName);
        }

        public async Task Execute(Message message, TelegramBotClient botClient, string previousMessage)
        {
            var chatId = message.Chat.Id;

            var userMessageRecord = userMessageService.Get(chatId);
            if (userMessageRecord == null)
            {
                userMessageService.Create(chatId, message.Text);
            }
            else
            {
                userMessageService.Update(chatId, message.Text);
            }

            if(previousMessage == Constants.SearchByStopName)
            {
                await botClient.SendTextMessageAsync(chatId, $"Тут мають вивестись всі маршрутки по назві зупинки");
            }
            else
            {
                await botClient.SendTextMessageAsync(chatId, $"ERRROOOOR");
            }
        }
    }
}
