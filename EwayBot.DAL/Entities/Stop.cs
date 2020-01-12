using System;
using System.Collections.Generic;
using System.Text;

namespace EwayBot.DAL.Entities
{
    public class Stop
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
