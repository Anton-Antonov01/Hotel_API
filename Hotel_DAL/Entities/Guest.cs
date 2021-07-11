using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_DAL.Entities
{
    public class Guest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        [JsonIgnore]
        public virtual ICollection<Booking> Bookings { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Guest)
            {
                var objRM = obj as Guest;
                return this.Id == objRM.Id
                    && this.Name == objRM.Name
                    && this.Address == objRM.Address
                    && this.Phone == objRM.Phone;
            }
            else
            {
                return base.Equals(obj);
            }
        }

    }
}
