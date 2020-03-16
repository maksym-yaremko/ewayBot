using EwayBot.BLL.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;

namespace EwayBot.BLL
{
    public class BotClient
    {
        public static TelegramBotClient botClient;

        public static List<ICommand> Commands => CommandsInitializer.commandsList;

        public static async Task<TelegramBotClient> GetBotClientAsync()
        {
            if (botClient != null)
            {
                return botClient;
            }

            botClient = new TelegramBotClient("1096673257:AAGq_sGCLZ2z-g6IPdtsTuwTcfj1qt0DfGM");
            string hook = string.Format("https://bb605ff1.ngrok.io/api/message/update");
            await botClient.SetWebhookAsync(hook);
            return botClient;
        }
    }
}
