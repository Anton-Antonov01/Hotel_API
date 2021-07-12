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
    public class CategoryController : ApiController
    {
        private ICategoryService service;
        private IMapper Mapper;

        public CategoryController(ICategoryService service)
        {
            this.service = service;

            Mapper = new MapperConfiguration(
                cfg => {
                    cfg.CreateMap<CategoryDTO, CategoryModel>().ReverseMap();
                    cfg.CreateMap<CategoryRequest, CategoryDTO>().ReverseMap();
                    }
                ).CreateMapper();
        }

        /// <summary>
        /// Вывод всех категорий 
        /// </summary>
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            IEnumerable<CategoryDTO> categoryDTOList = service.GetAllCategories();

            return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<IEnumerable<CategoryDTO>, IEnumerable<CategoryModel>>(categoryDTOList));
        }

        /// <summary>
        /// Вывод одной категории по Id 
        /// </summary>
        [ResponseType(typeof(CategoryModel))]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            try
            {
                var categoryModel = Mapper.Map<CategoryDTO, CategoryModel>(service.Get(id));
                return request.CreateResponse(HttpStatusCode.OK, categoryModel);
            }
            catch (NullReferenceException ex)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Добавление категории в БД
        /// </summary>
        public HttpResponseMessage Post(HttpRequestMessage request, [FromBody] CategoryRequest category)
        {
            try
            {
                if (!ModelState.IsValid)
                    return request.CreateResponse(HttpStatusCode.BadRequest);

                service.AddCategory(Mapper.Map<CategoryRequest, CategoryDTO>(category));
                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ArgumentException ex)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Редактирование категории 
        /// </summary>
        public HttpResponseMessage Put(int id, HttpRequestMessage request ,[FromBody] CategoryRequest category)
        {
            if (!ModelState.IsValid)
                return request.CreateResponse(HttpStatusCode.BadRequest);

            var categoryDTO = Mapper.Map<CategoryRequest, CategoryDTO>(category);
            categoryDTO.Id = id;
            try
            {
                service.UpdateCategory(categoryDTO);
                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch(NullReferenceException ex)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (ArgumentException ex)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }


        /// <summary>
        /// Удаление категории из БД
        /// </summary>
        [HttpDelete]
        [ResponseType(typeof(CategoryModel))]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            try
            {
                service.DeleteCategory(id);
                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch(NullReferenceException ex)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
