using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Models;

namespace TravelApp.Services
{
    public class APIService : IAPIService
    {
        public string GetCityName(string CityName)
        {
            using (WebClient web = new WebClient())
            {
                web.Encoding = Encoding.UTF8;
                string url = $"https://api.teleport.org/api/cities/?search={CityName}&limit=1";
                string json = "";

                try
                {
                    json = web.DownloadString(url);
                }
                catch (WebException ex)
                {
                    throw ex;
                }
                var jsObj = JObject.Parse(json);
                var count = (int)jsObj["count"];

                if (count == 0)
                    return "";

                var geonameUri = (string)jsObj["_embedded"]["city:search-results"][0]["_links"]["city:item"]["href"];

                try
                {
                    json = web.DownloadString($"{geonameUri}");
                }
                catch (WebException ex)
                {
                    throw ex;
                }
                var jsObj_city = JObject.Parse(json);

                return (string)jsObj_city["name"];
            }
        }

        public City GetCity(string CityName)
        {
            var city = new City();

            using (WebClient web = new WebClient())
            {
                web.Encoding = Encoding.UTF8;
                string url = $"https://api.teleport.org/api/cities/?search={CityName}&limit=1";
                string json = "";

                try
                {
                    json = web.DownloadString(url);
                }
                catch (WebException ex)
                {
                    throw ex;
                }
                var jsObj = JObject.Parse(json);
                //var count = (int)jsObj["count"];

                //if (count == 0)
                //    return city;

                var geonameUri = (string)jsObj["_embedded"]["city:search-results"][0]["_links"]["city:item"]["href"];

                try
                {
                    json = web.DownloadString($"{geonameUri}");
                }
                catch (WebException ex)
                {
                    throw ex;
                }
                var jsObj_city = JObject.Parse(json);

                city.CityName = (string)jsObj_city["name"];
                city.Latitude = (string)jsObj_city["location"]["latlon"]["latitude"];
                city.Longitude = (string)jsObj_city["location"]["latlon"]["longitude"];
                city.CountryName = (string)jsObj_city["_links"]["city:country"]["name"];

                var countryUri = (string)jsObj_city["_links"]["city:country"]["href"];
                try
                {
                    json = web.DownloadString($"{countryUri}");
                }
                catch (WebException ex)
                {
                    throw ex;
                }
                var jsObj_country = JObject.Parse(json);

                city.Currency = (string)jsObj_country["currency_code"];
                city.CountryCode = (string)jsObj_country["iso_alpha2"];
                //continent.Content = (string)jsObj_country["_links"]["country:continent"]["name"];

                var tzUri = (string)jsObj_city["_links"]["city:timezone"]["href"];
              //  tz.Content = (string)jsObj_city["_links"]["city:timezone"]["name"];

                try
                {
                    json = web.DownloadString($"{tzUri}");
                }
                catch (WebException ex)
                {
                    throw ex;
                }
                var jsObj_tz = JObject.Parse(json);
                tzUri = (string)jsObj_tz["_links"]["tz:offsets-now"]["href"];

                try
                {
                    json = web.DownloadString($"{tzUri}");
                }
                catch (WebException ex)
                {
                    throw ex;
                }
                var timezone = JObject.Parse(json);
                city.TimeZoneShortName = (string)timezone["short_name"];
                city.TimeZone = (int)timezone["base_offset_min"]/60;

                var UrbanArea = jsObj_city["_links"]["city:urban_area"];
                if (UrbanArea != null)
                {
                    var UrbanAreaUri = (string)jsObj_city["_links"]["city:urban_area"]["href"];
                    try
                    {
                        json = web.DownloadString($"{UrbanAreaUri}");
                    }
                    catch (WebException ex)
                    {
                        throw ex;
                    }
                    var UrbanAreaObj = JObject.Parse(json);

                    city.Mayor = (string)UrbanAreaObj["mayor"];

                    var imageUri = (string)UrbanAreaObj["_links"]["ua:images"]["href"];

                    try
                    {
                        json = web.DownloadString($"{imageUri}");
                    }
                    catch (WebException ex)
                    {
                        throw ex;
                    }
                    var images = JObject.Parse(json);

                    city.ImageUri = (string)images["photos"][0]["image"]["web"];
                }
                return city;
            }
        }
    }
}
