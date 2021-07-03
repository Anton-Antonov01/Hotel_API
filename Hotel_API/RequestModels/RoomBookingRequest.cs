using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotel_API.RequestModels
{
    public class RoomBookingRequest
    {
        public DateTime BookingDate { get; set; }
        public DateTime EnterDate { get; set; }
        public int NumberOfDays { get; set; }
        public int GuestId { get; set; }
        public int RoomId { get; set; }
        public bool Set { get; set; }
    }
}