using EwayBot.BLL;
using EwayBot.BLL.Helpers;
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
            foreach (var command in commands)
            {
                if (command.Contains(message))
                {
                    await command.Execute(message, botClient);
                    break;
                }
            }
            
            return Ok();
        }
    }
}
