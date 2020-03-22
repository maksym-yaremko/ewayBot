using EwayBot.DAL.Constants;
using EwayBot.DAL.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace EwayBot.BLL.Commands
{
    public class SearchStopByYourLocation : ICommand
    {
        public UserMessageService userMessageService { get; set; }
        public SearchStopByYourLocation()
        {
            userMessageService = new UserMessageService();
        }
        public bool Contains(Message message, string previousMessage)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Constants.SearchStopByYourLocation);
        }

        public async Task Execute(Message message, TelegramBotClient botClient, string previousMessage = null)
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

            var inlineKeyboard = new ReplyKeyboardMarkup(new[]
            {
                        new []
                        {
                            new KeyboardButton("Location")
                            { 
                                RequestLocation = true
                            },
                        }
            },resizeKeyboard:true);

            await botClient.SendTextMessageAsync(chatId, $"Нажміть на кнопку та скиньте свою локацію 📍 ⤵️", parseMode: ParseMode.Markdown,replyMarkup: inlineKeyboard);
        }
    }
}
