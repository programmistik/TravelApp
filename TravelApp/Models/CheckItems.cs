using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    public class CheckItem
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public bool ItemStatus { get; set; }
      //  [Key, ForeignKey("Trip")]
        public int? TripId { get; set; }
       // [ForeignKey("TripId")]
        public virtual Trip Trip { get; set; }
    }
}
