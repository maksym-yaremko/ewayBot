using System;
using System.Collections.Generic;
using System.Text;

namespace EwayBot.DAL.Models
{
    public class Transport
    {
        public Attributes Attributes { get; set; }
        public List<Route> Route { get; set; }
    }
}
