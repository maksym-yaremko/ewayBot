using EwayBot.BLL;
using EwayBot.BLL.Helpers;
using EwayBot.DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace EwayBot.Controllers
{
    [Route("api/message/update")]
    public class MessageController : Controller
    {
        public UserMessageService userMessageService { get; set; }
        public MessageController()
        {
            userMessageService = new UserMessageService();
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

            var commands = BotClient.Commands;

            var botClient = await BotClient.GetBotClientAsync();

            if (update == null)
            {
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
