using System.Collections.Generic;

namespace EwayBot.BLL.EwayAPI.Models
{
    public class GetStopInfoModel
    {
        public List<Stop> stop { get; set; }
    }

    public class Attributes
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
    }

    public class Route
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Has_Gps { get; set; }
        public int? Next_Vehicle { get; set; }
        public int? Second_Vehicle { get; set; }
    }

    public class Transport
    {
        public Attributes Attributes { get; set; }
        public List<Route> Route { get; set; }
    }

    public class Transports
    {
        public List<Transport> Transport { get; set; }
    }

    public class Stop
    {
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Title { get; set; }
        public Transports Transports { get; set; }
    }
}
