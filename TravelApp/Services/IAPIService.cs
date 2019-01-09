using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Models;

namespace TravelApp.Services
{
    public interface IAPIService
    {
        City GetCity(string CityName);
        string GetCityName(string CityName);
    }
}
