using EwayBot.BLL.Helpers;
using EwayBot.DAL.Constants;
using EwayBot.DAL.Services;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace EwayBot.BLL.Commands
{
    public class InfoCommand : ICommand
    {
        public UserMessageService userMessageService { get; set; }
        public InfoCommand()
        {
            userMessageService = new UserMessageService();
        }
        public bool Contains(Message message,string previousMessage)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Constants.Info);
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



            await botClient.SendTextMessageAsync(chatId, $"Тобі доступні наступні команди:\n🚏 /searchByStopName - пошук за назвою зупинки \n📍 /searchStopByYourLocation - пошук за локацією");


        }
    }
}
