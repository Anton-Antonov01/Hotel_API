using AutoMapper;
using Hotel_API.Models;
using Hotel_BL.DTO;
using Hotel_BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Hotel_API.RequestModels;

namespace Hotel_API.Controllers
{
    public class PriceCategoryController : ApiController
    {
        IPriceCategoryService priceCategoryService;
        ICategoryService categoryService;
        IMapper Mapper;

        public PriceCategoryController(IPriceCategoryService priceCategoryService, ICategoryService categoryService)
        {
            this.priceCategoryService = priceCategoryService;
            this.categoryService = categoryService;

            Mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<PriceCategoryDTO, PriceCategoryModel>().ReverseMap();
                    cfg.CreateMap<CategoryDTO, CategoryModel>().ReverseMap();
                    cfg.CreateMap<PriceCategoryRequest, PriceCategoryDTO>();
                }).CreateMapper();
        }

        /// <summary>
        /// Вывод всех ценовых категорий
        /// </summary>
        public IEnumerable<PriceCategoryModel> Get()
        {
            var data = priceCategoryService.GetAllPriceCategories();
            return Mapper.Map<IEnumerable<PriceCategoryDTO>, IEnumerable<PriceCategoryModel>>(data);
        }

        /// <summary>
        /// Вывод одной ценовой категории по Id
        /// </summary>
        [ResponseType(typeof(PriceCategoryModel))]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            try
            {
                var priceCategoryDTO = priceCategoryService.Get(id);
                var priceCategoryModel = Mapper.Map<PriceCategoryDTO, PriceCategoryModel>(priceCategoryDTO);
                return request.CreateResponse(HttpStatusCode.OK, priceCategoryModel);
            }
            catch (NullReferenceException ex)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Добавление ценовой категории в БД
        /// </summary>
        public HttpResponseMessage Post(HttpRequestMessage request,[FromBody] PriceCategoryRequest priceCategory)
        {
            if (!ModelState.IsValid)
                return request.CreateResponse(HttpStatusCode.BadRequest);

            PriceCategoryDTO priceCategoryDTO = Mapper.Map<PriceCategoryRequest, PriceCategoryDTO>(priceCategory);

            try
            {
                var category = categoryService.Get(priceCategory.CategoryId);
                priceCategoryDTO.Category = category;

                priceCategoryService.AddPriceCategory(priceCategoryDTO);
                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ArgumentException ex)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (NullReferenceException ex)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }

        /// <summary>
        /// Редактирование ценовой категории в БД
        /// </summary>
        public HttpResponseMessage Put(HttpRequestMessage request, int id, [FromBody] PriceCategoryRequest priceCategory)
        {
            if (!ModelState.IsValid)
                return request.CreateResponse(HttpStatusCode.BadRequest);

            PriceCategoryDTO priceCategoryDTO = Mapper.Map<PriceCategoryRequest, PriceCategoryDTO>(priceCategory);

            try
            {
                var category = categoryService.Get(priceCategory.CategoryId);
                priceCategoryDTO.Category = category;
                priceCategoryDTO.Id = id;

                priceCategoryService.UpdatePriceCategory(priceCategoryDTO);
                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ArgumentException ex)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Удаление ценовой категории из БД
        /// </summary>
        [HttpDelete]
        [ResponseType(typeof(PriceCategoryModel))]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            try
            {
                priceCategoryService.DeletePriceCategory(id);
                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (NullReferenceException ex)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
