using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EwayBot.BLL.Buttons
{
    public interface IButton
    {
        public Task Execute(Message message, TelegramBotClient botClient, string previousMessage = null);

        public bool Contains(Message message, string previousMessage = null);
    }
}
