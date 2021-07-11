using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_BL.DTO
{
    public class GuestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is GuestDTO)
            {
                var objRM = obj as GuestDTO;
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
