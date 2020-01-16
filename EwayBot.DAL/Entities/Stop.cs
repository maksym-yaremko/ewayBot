using System;
using System.Collections.Generic;
using System.Text;

namespace EwayBot.DAL.Entities
{
    public class Stop
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
    }
}
