using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_DAL.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Bed { get; set; }

        [JsonIgnore]
        public virtual ICollection<Room> Rooms { get; set; }

        [JsonIgnore]
        public virtual ICollection<PriceCategory> PriceCategories { get; set; }
    }
}
