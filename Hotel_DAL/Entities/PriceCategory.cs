using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_DAL.Entities
{
    public class PriceCategory
    {
        [Key]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        [ForeignKey("CategoryId")]
        [JsonIgnore]
        public virtual Category Category { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is PriceCategory)
            {
                var objPC = obj as PriceCategory;
                return this.Id == objPC.Id
                    && this.Price == objPC.Price
                    && this.StartDate == objPC.StartDate
                    && this.EndDate == objPC.EndDate
                    && this.CategoryId == objPC.CategoryId;
                //Добавить сравнение категории
            }
            else
            {
                return base.Equals(obj);
            }
        }
    }
}
