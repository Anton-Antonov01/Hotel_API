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
    class CategoryRepository : IRepository<Category>
    {
        HotelContext db;

        public CategoryRepository(HotelContext db)
        {
            this.db = db;
        }

        public IEnumerable<Category> GetAll()
        {
            return db.Categories;
        }

        public Category Get(int id)
        {
            return db.Categories.Find(id);
        }

        public void Create(Category category)
        {
            db.Categories.Add(category);
        }

        public void Delete(int id)
        {
            Category category = Get(id);
            if (category != null)
                db.Categories.Remove(category);
        }

        public void Update(Category category)
        {
            var toUpdate = db.Categories.FirstOrDefault(m => m.Id == category.Id);
            if (toUpdate != null)
            {
                toUpdate.Name = category.Name ?? toUpdate.Name;
                toUpdate.Bed = category.Bed;
            }
        }
    }
}
