using EwayBot.DAL.Constants;
using EwayBot.DAL.Services;
using EwayBot.DTO;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace EwayBot.BLL.Commands
{
    public class StopTextCommand : ICommand
    {
        public UserMessageService userMessageService { get; set; }
        public EwayApiService ewayApiService { get; set; }
        public StopTextCommand(IOptions<SensitiveTokens> sensitiveTokens)
        {
            userMessageService = new UserMessageService();
            ewayApiService = new EwayApiService(sensitiveTokens);
        }
        public bool Contains(Message message, string previousMessage)
        {
            if (message.Type != MessageType.Text)
                return false;

            return previousMessage.Contains(Constants.SearchByStopName) && !(message.Text.Contains(Constants.Start) || message.Text.Contains(Constants.Info) || message.Text.Contains(Constants.SearchByStopName) || message.Text.Contains(Constants.SearchByTransportNumber) || message.Text.Contains(Constants.SearchStopByYourLocation));
        }

        public async Task Execute(Message message, TelegramBotClient botClient, string previousMessage)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Обрати", "/stopMapCallback"),
                        }
                    });

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

            if(previousMessage == Constants.SearchByStopName)
            {
                
                StopService stopService = new StopService();
                var locations = stopService.GetLocation(message.Text);

                foreach(var location in locations)
                {
                    var result = await ewayApiService.GetStopInfo(location.Item4);
                    await botClient.SendVenueAsync(chatId, float.Parse(location.Item1, CultureInfo.InvariantCulture.NumberFormat), float.Parse(location.Item2, CultureInfo.InvariantCulture.NumberFormat),location.Item3,location.Item4, replyMarkup: inlineKeyboard);
                }
            }
            else
            {
                await botClient.SendTextMessageAsync(chatId, $"ERRROOOOR");
            }
        }
    }
}
