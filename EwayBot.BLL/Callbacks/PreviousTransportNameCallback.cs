using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EwayBot.BLL.Callbacks
{
    public class PreviousTransportNameCallback : ICallback
    {
        public bool Contains(Update fullObject, string previousMessage = null)
        {
            throw new NotImplementedException();
        }

        public Task Execute(TelegramBotClient botClient, Update fullObject, string previousMessage = null)
        {
            throw new NotImplementedException();
        }
    }
}
