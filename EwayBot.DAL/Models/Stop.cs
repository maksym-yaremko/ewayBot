using System;
using System.Collections.Generic;
using System.Text;

namespace EwayBot.DAL.Models
{
    public class Stop
    {
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Title { get; set; }
        public Transports Transports { get; set; }
    }
}
