using Hotel_BL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_BL.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<CategoryDTO> GetAllCategories();
        CategoryDTO Get(int id);
        void AddCategory(CategoryDTO categoryDTO);
        void DeleteCategory(int id);
        void UpdateCategory(CategoryDTO categoryDTO);
    }
}
