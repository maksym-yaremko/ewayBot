using EwayBot.DAL.Context;
using EwayBot.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EwayBot.DAL.Services
{
    public class StopService
    {
        public ApplicationContext db { get; set; }
        public StopService()
        {
            db = new ApplicationContext();
        }

        public IEnumerable<Tuple<string,string,string,string>> GetLocation(string stopName)
        {
            var locations = db.Stops.ToList().Where(x=>x.Title.ToLower().Contains(stopName.ToLower())).Select(res => Tuple.Create(res.Lat, res.Lng,res.Title,res.Id));
            return locations;
        }

        public Stop GetStopByLocation(string lat,string lng)
        {
            var stop = db.Stops.SingleOrDefault(x => x.Lat == lat && x.Lng == lng);
            return stop;
        }

        public Stop GetById(string stopId)
        {
            var stop = db.Stops.SingleOrDefault(x => x.Id==stopId);
            return stop;
        }
    }
}
