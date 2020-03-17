using EwayBot.DAL.Constants;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EwayBot.BLL.Callbacks
{
    public class StopChooseCallback : ICallback
    {
        public bool Contains(Update fullObject, string previousMessage = null)
        {
            //if (fullObject.Message.Type != MessageType.Text)
            //    return false;

            return fullObject.CallbackQuery.Data.Contains(Constants.StopMapCallback);
        }

        public async Task Execute(TelegramBotClient botClient, Update fullObject, string previousMessage = null)
        {
            var chatId = fullObject.CallbackQuery.Message.Chat.Id;
            var callBackQueryId = fullObject.CallbackQuery.Id;

            await botClient.SendTextMessageAsync(chatId, $"Вивести всі дані по маршрутках і прибуттю", parseMode: ParseMode.Markdown);
            //await botClient.AnswerCallbackQueryAsync(callBackQueryId);
            
        }
    }
}
