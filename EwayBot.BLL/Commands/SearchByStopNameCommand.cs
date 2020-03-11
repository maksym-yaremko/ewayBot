using EwayBot.BLL.Helpers;
using EwayBot.DAL.Constants;
using EwayBot.DAL.Services;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EwayBot.BLL.Commands
{
    public class SearchByStopNameCommand : ICommand
    {
        public UserMessageService userMessageService { get; set; }
        public SearchByStopNameCommand()
        {
            userMessageService = new UserMessageService();
        }
        public bool Contains(Message message, string previousMessage)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Constants.SearchByStopName);
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

            await botClient.SendTextMessageAsync(chatId, $"Введіть назву зупинки ⤵️", parseMode: ParseMode.Markdown);
        }
    }
}
