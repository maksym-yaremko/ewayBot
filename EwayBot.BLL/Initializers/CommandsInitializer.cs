using EwayBot.BLL.Commands;
using EwayBot.DTO;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace EwayBot.BLL.Initializers
{
    public class CommandsInitializer
    {
        public List<ICommand> commandsList { get; set; }
        public CommandsInitializer(IOptions<SensitiveTokens> sensitiveTokens)
        {
            commandsList = new List<ICommand>
            {
            new StartCommand(),
            new InfoCommand(),
            new SearchByStopNameCommand(),
            new StopTextCommand(sensitiveTokens),
            new SearchStopByYourLocation(),
            new SimpleTextCommand()
            };
        }
    }
}
