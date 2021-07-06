using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_BL.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Bed { get; set; }


        public override bool Equals(object obj)
        {
            if (obj is CategoryDTO)
            {
                var objCM = obj as CategoryDTO;
                return this.Id == objCM.Id
                    && this.Name == objCM.Name
                    && this.Bed == objCM.Bed;
            }
            else
            {
                return base.Equals(obj);
            }
        }
    }
}
