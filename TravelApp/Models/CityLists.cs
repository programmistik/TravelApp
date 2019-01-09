using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    public class CityList
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
    }
}
