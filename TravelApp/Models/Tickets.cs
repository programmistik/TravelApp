using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string TicketName { get; set; }
        public string TicketUri { get; set; }
       // [Key, ForeignKey("Trip")]
        public int? TripId { get; set; }
       // [ForeignKey("TripId")]
        public virtual Trip Trip { get; set; }
    }
}
