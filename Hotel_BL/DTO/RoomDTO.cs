using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_BL.DTO
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CategoryDTO RoomCategory { get; set; }
        public bool Active { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is RoomDTO)
            {
                var objRDTO = obj as RoomDTO;
                return this.Id == objRDTO.Id
                    && this.Name == objRDTO.Name
                    && this.Active == objRDTO.Active;
            }
            else
            {
                return base.Equals(obj);
            }
        }

    }
}
