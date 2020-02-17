using EwayBot.DAL.Context;
using EwayBot.DAL.Entities;
using System.Linq;

namespace EwayBot
{
    public class SampleData
    {
        public static void Initialize(ApplicationContext context)
        {
            if (!context.Stops.Any())
            {
                context.Stops.AddRange(
                    new Stop
                    {
                        Title="1"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
