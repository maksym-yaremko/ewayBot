using EwayBot.DAL.Constants;
using EwayBot.DAL.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EwayBot.BLL.Commands
{
    public class SimpleTextCommand : ICommand
    {
        public UserMessageService userMessageService { get; set; }
        public SimpleTextCommand()
        {
            userMessageService = new UserMessageService();
        }
        public bool Contains(Message message, string previousMessage)
        {
            if (message.Type != MessageType.Text)
                return false;

            return previousMessage.Contains(Constants.SearchByStopName);
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

            if(previousMessage == Constants.SearchByStopName)
            {
                using var client = new HttpClient();
                
                StopService stopService = new StopService();
                var locations = stopService.GetLocation(message.Text);

                foreach(var location in locations)
                {
                    var result =await client.GetAsync(@$"https://api.eway.in.ua/?login=smoliakandriy&password=m3ns2h36frT9c2Aqj&function=stops.GetStopInfo&city=lviv&id={location.Item4}");
                    var res = result.Content.ReadAsStringAsync();
                    await botClient.SendVenueAsync(chatId, float.Parse(location.Item1, CultureInfo.InvariantCulture.NumberFormat), float.Parse(location.Item2, CultureInfo.InvariantCulture.NumberFormat),location.Item3,location.Item4);
                }
            }
            else
            {
                await botClient.SendTextMessageAsync(chatId, $"ERRROOOOR");
            }
        }
    }
}
