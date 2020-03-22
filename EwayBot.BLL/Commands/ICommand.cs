using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EwayBot.BLL.Commands
{
    public interface ICommand
    {
        public Task Execute(Message message, TelegramBotClient botClient, string previousMessage = null);

        public bool Contains(Message message, string previousMessage = null);
    }
}
