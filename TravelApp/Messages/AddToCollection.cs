using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Models;

namespace TravelApp.Messages
{
    public class AddToCollection
    {
        public bool Add { get; set; }
        public Trip NewTrip { get; set; }

        public AddToCollection(bool add, Trip newTrip)
        {
            Add = add;
            NewTrip = newTrip;
        }
    }
    
}
