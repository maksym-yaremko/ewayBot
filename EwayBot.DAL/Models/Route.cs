using System;
using System.Collections.Generic;
using System.Text;

namespace EwayBot.DAL.Models
{
    public class Route
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Start_Position { get; set; }
        public string Stop_Position { get; set; }
        public string Transport { get; set; }
        public string Gps_Id { get; set; }
        public string Has_Gps { get; set; }
        public int Gps_Count { get; set; }
        public int? Next_Vehicle { get; set; }
        public int? Second_Vehicle { get; set; }
        public string Type { get; set; }
        public int Is_Suburban { get; set; }
        public string Price { get; set; }
        public string Interval { get; set; }
        public string Work_Time { get; set; }
        public string Refresh_Date { get; set; }
        public string Feature { get; set; }
        public string Short_Description { get; set; }
        public string Description { get; set; }

    }
}
