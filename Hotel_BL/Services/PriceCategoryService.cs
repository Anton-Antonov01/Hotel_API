using AutoMapper;
using Hotel_BL.DTO;
using Hotel_BL.HelpClasses;
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
    public class PriceCategoryService : IPriceCategoryService
    { 
        IWorkUnit Database;
        IMapper Mapper;

        public PriceCategoryService(IWorkUnit database)
        {
            Database = database;
            Mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<PriceCategory, PriceCategoryDTO>().ReverseMap();
                    cfg.CreateMap<Category, CategoryDTO>().ReverseMap();
                }).CreateMapper();
        }


        public PriceCategoryDTO Get(int id)
        {
            var priceCategory = Database.PriceCategories.Get(id);
            if (priceCategory == null)
                throw new ArgumentException();

            return Mapper.Map<PriceCategory, PriceCategoryDTO>(priceCategory);
        }

        public IEnumerable<PriceCategoryDTO> GetAllPriceCategories()
        {
            var priceCategories = Database.PriceCategories.GetAll();

            return Mapper.Map<IEnumerable<PriceCategory>, IEnumerable<PriceCategoryDTO>>(priceCategories);

        }

        public void AddPriceCategory(PriceCategoryDTO priceCategoryDTO)
        {
            if (priceCategoryDTO.Category == null)
                throw new ArgumentException();

            if (priceCategoryDTO.EndDate < priceCategoryDTO.StartDate)
                throw new ArgumentException();

            if (!CheckValidDatePriceCategory(priceCategoryDTO))
                throw new ArgumentException();

            Database.PriceCategories.Create(Mapper.Map<PriceCategoryDTO, PriceCategory>(priceCategoryDTO));
            Database.Save();
        }

        public void UpdatePriceCategory(PriceCategoryDTO priceCategoryDTO)
        {
            if (priceCategoryDTO.EndDate < priceCategoryDTO.StartDate)
                throw new ArgumentException();

            if (priceCategoryDTO.Category == null)
                throw new ArgumentException();

            if(!CheckValidDatePriceCategory(priceCategoryDTO))
                throw new ArgumentException();


            Database.PriceCategories.Update(Mapper.Map<PriceCategoryDTO, PriceCategory>(priceCategoryDTO));
            Database.Save();
        }

        public void DeletePriceCategory(int id)
        {
            if (Database.PriceCategories.Get(id) == null)
                throw new ArgumentException();


            Database.PriceCategories.Delete(id);
            Database.Save();
        }

        private bool CheckValidDatePriceCategory(PriceCategoryDTO priceCategoryDTO)
        {
            var AllPriceCategoriesToCategory = Database.PriceCategories.GetAll().Where(p => p.CategoryId == priceCategoryDTO.Category.Id);

            Interval PriceCategoryInterval = new Interval();
            PriceCategoryInterval.Start = priceCategoryDTO.StartDate;
            PriceCategoryInterval.End = priceCategoryDTO.EndDate;

            Interval ExsistingPriceCategoryInterval = new Interval();

            foreach (var p in AllPriceCategoriesToCategory)
            {
                ExsistingPriceCategoryInterval.Start = p.StartDate;
                ExsistingPriceCategoryInterval.End = p.EndDate;

                if (PriceCategoryInterval.IsInclude(ExsistingPriceCategoryInterval))
                    return false;
            }

            return true;
        }

    }
}
