using Hotel_BL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotel_API.Models
{
    public class RoomModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CategoryModel RoomCategory { get; set; }
        public bool Active { get; set; }
    }
}