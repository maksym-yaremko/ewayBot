using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EwayBot.BLL.Callbacks
{
    public interface ICallback
    {
        public Task Execute(TelegramBotClient botClient, Update fullObject, string previousMessage = null);

        public bool Contains(Update fullObject, string previousMessage = null);
    }
}
