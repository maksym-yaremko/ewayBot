using EwayBot.BLL;
using EwayBot.BLL.Helpers;
using EwayBot.DAL.Services;
using EwayBot.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EwayBot.Controllers
{
    [Route("api/message/update")]
    public class MessageController : Controller
    {
        public UserMessageService userMessageService { get; set; }
        public BotClient _botClient { get; set; }
        public MessageController(IOptions<SensitiveTokens> sensitiveTokens)
        {
            userMessageService = new UserMessageService();
            _botClient = new BotClient(sensitiveTokens);
        }
        public string lastMessage { get; set; }

        [HttpGet]
        public string Get()
        {
            return "Method GET unuvalable";
        }
        [HttpPost]
        public async Task<OkResult> Post([FromBody]object obj)
        {
            
            var update = new Update();
            try
            {
                update = JsonConvert.DeserializeObject<Update>(obj.ToString(), new MyDateTimeConverter());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            var commands = _botClient.Commands;
            var callbacks = _botClient.Callbacks;
            var buttons = _botClient.Buttons;

            var botClient = await _botClient.GetBotClientAsync();

            if (update == null)
            {
                return Ok();
            }


            if(update.Type == UpdateType.CallbackQuery)
            {
                foreach (var callback in callbacks)
                {
                    if (callback.Contains(update))
                    {
                        await callback.Execute(botClient,update);
                        break;
                    }
                }
                return Ok();
            }

            if(update.Message.Type == MessageType.Location)
            {
                var mes = update.Message;

                foreach (var button in buttons)
                {
                    if (button.Contains(mes))
                    {
                        await button.Execute(mes, botClient);
                        break;
                    }
                }

                return Ok();
            }

            var message = update.Message;
            var previousMessage = userMessageService.Get(update.Message.Chat.Id);
            if (previousMessage == null)
            {
                userMessageService.Create(update.Message.Chat.Id, update.Message.Text);
            }
            var newPreviousMessage = userMessageService.Get(update.Message.Chat.Id);

            foreach (var command in commands)
            {
                if (command.Contains(message, newPreviousMessage.Message))
                {
                    await command.Execute(message, botClient, newPreviousMessage.Message);
                    break;
                }
            }
            
            return Ok();
        }
    }
}
