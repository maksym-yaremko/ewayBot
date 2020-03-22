using System;
using System.Collections.Generic;
using System.Text;

namespace EwayBot.DAL.ModelsNewVersion
{
    public class GetStopInfoModelNewVersion
    {
        public int id { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public string title { get; set; }
        public List<RouteNewVersion> routes { get; set; }
    }
}
