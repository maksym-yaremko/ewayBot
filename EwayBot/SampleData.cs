using EwayBot.DAL.Context;
using EwayBot.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
