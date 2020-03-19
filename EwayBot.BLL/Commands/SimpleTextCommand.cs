using EwayBot.DAL.Constants;
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
        public bool Contains(Message message, string previousMessage = null)
        {
            if (message.Type != MessageType.Text)
                return false;

            return !previousMessage.Contains(Constants.SearchByStopName);
        }

        public async Task Execute(Message message, TelegramBotClient botClient, string previousMessage = null)
        {
            var chatId = message.Chat.Id;

            await botClient.SendTextMessageAsync(chatId, $"Виберіть одну з наступних команд:\n🚏 /searchByStopName - пошук за назвою зупинки \n📍 /searchStopByYourLocation - пошук за локацією");

        }
    }
}
