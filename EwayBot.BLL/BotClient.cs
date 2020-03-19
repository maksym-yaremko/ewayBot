using EwayBot.BLL.Buttons;
using EwayBot.BLL.Callbacks;
using EwayBot.BLL.Commands;
using EwayBot.BLL.Initializers;
using EwayBot.DTO;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;

namespace EwayBot.BLL
{
    public class BotClient
    {
        public static TelegramBotClient botClient;
        public CommandsInitializer commandsInitializer;
        public CallbacksInitializer callbacksInitializer;
        public ButtonsInitializer buttonsInitializer;
        public BotClient(IOptions<SensitiveTokens> sensitiveTokens)
        {
            commandsInitializer = new CommandsInitializer(sensitiveTokens);
            callbacksInitializer = new CallbacksInitializer(sensitiveTokens);
            buttonsInitializer = new ButtonsInitializer(sensitiveTokens);
        }

        public List<ICommand> Commands => commandsInitializer.commandsList;
        public List<ICallback> Callbacks => callbacksInitializer.callbacksList;
        public List<IButton> Buttons => buttonsInitializer.buttonsList;


        public async Task<TelegramBotClient> GetBotClientAsync()
        {
            if (botClient != null)
            {
                return botClient;
            }

            botClient = new TelegramBotClient("1096673257:AAGq_sGCLZ2z-g6IPdtsTuwTcfj1qt0DfGM");
            string hook = string.Format("https://8e2eccd8.ngrok.io/api/message/update");
            await botClient.SetWebhookAsync(hook);
            return botClient;
        }
    }
}
