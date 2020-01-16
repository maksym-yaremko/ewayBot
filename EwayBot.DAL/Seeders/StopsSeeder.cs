using EwayBot.DAL.Context;
using EwayBot.DAL.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EwayBot.DAL.Seeders
{
    public class StopsSeeder
    {
        public StringBuilder street { get; set; }
        public List<string> ids { get; set; }
        public List<string> streets { get; set; }
        public List<string> lat { get; set; }
        public List<string> lng { get; set; }
        public StopsSeeder()
        {
            street = new StringBuilder();
            ids = new List<string>();
            streets = new List<string>();
            lat = new List<string>();
            lng = new List<string>();
        }
        public void ParseData()
        {
            string[] lines = File.ReadAllLines("stops.txt");

            foreach (var line in lines)
            {
                string[] splited = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                lng.Add(splited[splited.Length - 1]);
                lat.Add(splited[splited.Length - 2]);
                ids.Add(splited[0]);

                for (int i = 1; i < splited.Length - 2; i++)
                {
                    street.Append(splited[i]);
                    street.Append(' ');
                }
                streets.Add(street.ToString());
                street = street.Clear();
            }
        }
        public void InitializeStops(ApplicationContext context)
        {
            ParseData();
            if (!context.Stops.Any())
            {
                for(int i = 0; i < ids.Count; i++) {
                    context.Stops.Add(
                        new Stop
                        {
                            Id = ids[i],
                            Title = streets[i],
                            Lng = lng[i],
                            Lat = lat[i]
                        }
                    );
                }
                context.SaveChanges();
            }
        }
    }
}
