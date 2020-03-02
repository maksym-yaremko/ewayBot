using System;
using System.Collections.Generic;
using System.Text;

namespace EwayBot.DAL.Models
{
    public class Route
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Has_Gps { get; set; }
        public int? Next_Vehicle { get; set; }
        public int? Second_Vehicle { get; set; }
    }
}
