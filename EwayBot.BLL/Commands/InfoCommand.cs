using EwayBot.DAL.Constants;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EwayBot.BLL.Commands
{
    public class InfoCommand : ICommand
    {
        public bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Constants.Info);
        }

        public async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            await botClient.SendTextMessageAsync(chatId, $"Тобі доступні наступні команди:\nПошук по назві зупинки /searchByStopName", parseMode: ParseMode.Markdown);
        }
    }
}
