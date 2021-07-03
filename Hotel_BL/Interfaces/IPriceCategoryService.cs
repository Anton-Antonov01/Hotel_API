using Hotel_BL.DTO;
using Hotel_DAL.Entities;
using Hotel_DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_BL.Interfaces
{
    public interface IPriceCategoryService
    {
        PriceCategoryDTO Get(int id);
        void AddPriceCategory(PriceCategoryDTO guestDTO);
        IEnumerable<PriceCategoryDTO> GetAllPriceCategories();
        void DeletePriceCategory(int id);
        void UpdatePriceCategory(PriceCategoryDTO priceCategoryDTO);
    }
}
