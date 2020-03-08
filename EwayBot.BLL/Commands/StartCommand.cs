using EwayBot.DAL.Constants;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EwayBot.BLL.Commands
{
    public class StartCommand : ICommand
    {
        public bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Constants.Start);
        }

        public async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            var fullName = message.From.FirstName + " " + message.From.LastName;
            await botClient.SendTextMessageAsync(chatId, $"Привіт,{fullName}, я - бот для відстеження громадського транспорту Львова🦁.\nДля перегляду доступних команд натисни /info", parseMode: ParseMode.Markdown);
        }
    }
}
