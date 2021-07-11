using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_BL.DTO
{
    public class PriceCategoryDTO
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public  CategoryDTO Category { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is PriceCategoryDTO)
            {
                var objCM = obj as PriceCategoryDTO;
                return this.Id == objCM.Id
                    && this.Price == objCM.Price
                    && this.StartDate == objCM.StartDate
                    && this.EndDate == objCM.EndDate;
                     //Добавить сравнение категории
            }
            else
            {
                return base.Equals(obj);
            }
        }

    }
}
