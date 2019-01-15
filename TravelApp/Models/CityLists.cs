using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        
        public int? TripId { get; set; }
        //[ForeignKey("TripId")]
        public virtual Trip Trip { get; set; }
    }
}
