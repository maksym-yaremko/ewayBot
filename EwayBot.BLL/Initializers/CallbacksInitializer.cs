using EwayBot.BLL.Callbacks;
using System.Collections.Generic;

namespace EwayBot.BLL.Initializers
{
    public class CallbacksInitializer
    {
        public List<ICallback> callbacksList { get; set; }
        public CallbacksInitializer()
        {
            callbacksList = new List<ICallback>
            {
            new StopChooseCallback()
            };
        }
    }
}
