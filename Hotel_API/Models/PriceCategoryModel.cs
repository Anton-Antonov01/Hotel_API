using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotel_API.Models
{
    public class PriceCategoryModel
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CategoryModel Category { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is PriceCategoryModel)
            {
                var objCM = obj as PriceCategoryModel;
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