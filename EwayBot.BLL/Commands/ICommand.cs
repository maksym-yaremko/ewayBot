using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EwayBot.BLL.Commands
{
    public interface ICommand
    {
        public Task Execute(Message message, TelegramBotClient botClient);

        public bool Contains(Message message);
    }
}
