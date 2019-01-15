using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Models;

namespace TravelApp.Messages
{
    public class AddNewTripMessage
    {
        public Trip NewTrip { get; set; }

        public AddNewTripMessage(Trip NewTrip)
        {
            this.NewTrip = NewTrip;
        }
    }
}
