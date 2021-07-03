using Hotel_DAL.EF;
using Hotel_DAL.Entities;
using Hotel_DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_DAL.Repositories
{
    class PriceCategoryRepository : IRepository<PriceCategory>
    {
        HotelContext db;

        public PriceCategoryRepository(HotelContext db)
        {
            this.db = db;
        }

        public PriceCategory Get(int id)
        {
            return db.PriceCategories.Find(id);
        }

        public IEnumerable<PriceCategory> GetAll()
        {
            return db.PriceCategories;
        }

        public void Create(PriceCategory priceCategory)
        {
            priceCategory.Category = db.Categories.Find(priceCategory.Category.Id);
            db.PriceCategories.Add(priceCategory);
        }

        public void Delete(int id)
        {
            db.PriceCategories.Remove(Get(id));
        }

        public void Update(PriceCategory priceCategory)
        {
            var toUpdate = db.PriceCategories.FirstOrDefault(m => m.Id == priceCategory.Id);
            if (toUpdate != null)
            {
                toUpdate.Category = db.Categories.Find(priceCategory.Category.Id);
                toUpdate.Id = priceCategory.Id;
                toUpdate.CategoryId = priceCategory.CategoryId;
                toUpdate.StartDate = priceCategory.StartDate;
                toUpdate.EndDate = priceCategory.EndDate;
                toUpdate.Price = priceCategory.Price;
            }
        }
    }
}
