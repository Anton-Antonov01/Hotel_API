using AutoMapper;
using Hotel_BL.DTO;
using Hotel_BL.Interfaces;
using Hotel_DAL.Entities;
using Hotel_DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_BL.Services
{
    public class CategoryService : ICategoryService
    {
        private IWorkUnit Database { get; set; }
        IMapper Mapper;

        public CategoryService(IWorkUnit database)
        {
            this.Database = database;
            Mapper = new MapperConfiguration(
                cfg => cfg.CreateMap<Category, CategoryDTO>().ReverseMap()
                ).CreateMapper();
        }

        public CategoryDTO Get(int id)
        {
            var category = Database.Categories.Get(id);
            if (category == null)
                throw new ArgumentException();

            return Mapper.Map<Category, CategoryDTO>(category);
        }

        public IEnumerable<CategoryDTO> GetAllCategories()
        {
            var categories = Database.Categories.GetAll();

            return Mapper.Map<IEnumerable<Category>,List<CategoryDTO>>(categories);
        }

        public void AddCategory(CategoryDTO categoryDTO)
        {
            Database.Categories.Create(Mapper.Map<CategoryDTO,Category>(categoryDTO));
            Database.Save();
        }

        public void DeleteCategory(int id)
        {
            if (Database.Categories.Get(id) == null)
                throw new ArgumentException();

            Database.Categories.Delete(id);
            Database.Save();
        }

        public void UpdateCategory(CategoryDTO categoryDTO)
        {
            if (Database.Categories.Get(categoryDTO.Id) == null)
                throw new ArgumentException();

            Database.Categories.Update(Mapper.Map<CategoryDTO,Category>(categoryDTO));
            Database.Save();
        }
    }
}
