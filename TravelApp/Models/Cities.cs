using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    public class City
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string Currency { get; set; }
        public string Mayor { get; set; }
        public string TimeZoneShortName { get; set; }
        public int TimeZone { get; set; }
        public string ImageUri { get; set; }
    }
}
