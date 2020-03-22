using EwayBot.BLL.Callbacks;
using EwayBot.DTO;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace EwayBot.BLL.Initializers
{
    public class CallbacksInitializer
    {
        public List<ICallback> callbacksList { get; set; }
        public CallbacksInitializer(IOptions<SensitiveTokens> sensitiveTokens)
        {
            callbacksList = new List<ICallback>
            {
            new StopChooseCallback(sensitiveTokens),
            new TtransportTypeChooseCallback(sensitiveTokens),
            new NextTransportNameCallback(sensitiveTokens),
            new PreviousTransportNameCallback(sensitiveTokens),
            new TransportNameChooseCallback(sensitiveTokens)
            };
        }
    }
}
