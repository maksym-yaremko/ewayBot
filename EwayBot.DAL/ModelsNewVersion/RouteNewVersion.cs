using System;
using System.Collections.Generic;
using System.Text;

namespace EwayBot.DAL.ModelsNewVersion
{
    public class RouteNewVersion
    {
        public int id { get; set; }
        public string title { get; set; }
        public string directionTitle { get; set; }
        public string transportName { get; set; }
        public string transportKey { get; set; }
        public bool isSuburban { get; set; }
        public bool hasGPS { get; set; }
        public string bortNumber { get; set; }
        public bool handicapped { get; set; }
        public bool wifi { get; set; }
        public bool hasSchedules { get; set; }
        public string timeLeft { get; set; }
        public string timeLeftFormatted { get; set; }
        public string timeSource { get; set; }
    }
}
