using System;
using System.Collections.Generic;
using System.Text;

namespace EwayBot.DAL.Models
{
    public class Vehicle
    {
        public int id { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public int direction { get; set; }
        public int data_relevance { get; set; }
        public int handicapped { get; set; }
        public int wifi { get; set; }
    }
}
