using EwayBot.BLL.Commands;
using System.Collections.Generic;

namespace EwayBot.BLL
{
    public static class CommandsInitializer
    {
        public static List<ICommand> commandsList = new List<ICommand>
        {
            new StartCommand(),
            new InfoCommand(),
            new SearchByStopNameCommand()
        };
    }
}
