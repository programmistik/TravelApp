using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public string TripName { get; set; }

       // [Key, ForeignKey("User")]        
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        
        public Trip()
        {
            CityList = new HashSet<CityList>();
            Tickets = new HashSet<Ticket>();
            CheckItems = new HashSet<CheckItem>();
        }
        public virtual ICollection<CityList> CityList { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<CheckItem> CheckItems { get; set; }
    }
}
