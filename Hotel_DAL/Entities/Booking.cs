﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_DAL.Entities
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime EnterDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public bool Set { get; set; }
        public int GuestId { get; set; }
        public int RoomId { get; set; }

        [ForeignKey("GuestId")]
        [JsonIgnore]
        public virtual Guest guest { get; set; }

        [ForeignKey("RoomId")]
        [JsonIgnore]
        public virtual Room room { get; set; }
    }
}
