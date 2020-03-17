using EwayBot.BLL.Buttons;
using EwayBot.BLL.Commands;
using EwayBot.DTO;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EwayBot.BLL.Initializers
{
    public class ButtonsInitializer
    {
        public List<IButton> buttonsList { get; set; }
        public ButtonsInitializer(IOptions<SensitiveTokens> sensitiveTokens)
        {
            buttonsList = new List<IButton>
            {
            new LocationButton(sensitiveTokens)
            };
        }
    }
}
