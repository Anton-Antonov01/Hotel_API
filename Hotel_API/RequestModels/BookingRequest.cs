using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotel_API.RequestModels
{
    public class BookingRequest
    {
        [Required]
        public DateTime BookingDate { get; set; }
        [Required]
        public DateTime EnterDate { get; set; }
        [Required]
        public DateTime LeaveDate { get; set; }
        [Required]
        public bool Set { get; set; }
        [Required]
        public int GuestId { get; set; }
        [Required]
        public int RoomId { get; set; }
    }




}