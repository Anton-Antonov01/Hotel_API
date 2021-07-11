using AutoMapper;
using Hotel_API.Controllers;
using Hotel_API.Models;
using Hotel_API.RequestModels;
using Hotel_BL.DTO;
using Hotel_BL.Interfaces;
using Hotel_BL.Services;
using Hotel_DAL.Entities;
using Hotel_DAL.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace Hotel.Tests.ControllerTests
{
    [TestClass]
    public class CategoryControllerTest
    {
        IMapper mapper;
        Mock<IWorkUnit> EFWorkUnitMock;
        Mock<ICategoryService> CategoryServiceMock;

        HttpConfiguration httpConfiguration;
        HttpRequestMessage httpRequest;

        public CategoryControllerTest()
        {
            mapper = new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<Category, CategoryDTO>();
                cfg.CreateMap<CategoryDTO, CategoryModel>();
            }).CreateMapper();

            EFWorkUnitMock = new Mock<IWorkUnit>();
            CategoryServiceMock = new Mock<ICategoryService>();


            httpConfiguration = new HttpConfiguration();
            httpRequest = new System.Net.Http.HttpRequestMessage();
            httpRequest.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = httpConfiguration;
        }

        [TestMethod]
        public void GuestControllerGetTest()
        {
            int CategoryId = 1;

            EFWorkUnitMock.Setup(x => x.Categories.Get(CategoryId)).Returns(new Category());
            CategoryServiceMock.Setup(a => a.Get(CategoryId)).Returns(new CategoryDTO());

            var categoryService = new CategoryService(EFWorkUnitMock.Object);
            CategoryController controller = new CategoryController(CategoryServiceMock.Object);

            var httpResponse = controller.Get(httpRequest, CategoryId);
            var result = httpResponse.Content.ReadAsAsync<CategoryModel>();
            CategoryModel expected = mapper.Map<CategoryDTO, CategoryModel>(categoryService.Get(CategoryId));
            Assert.AreEqual(expected, result.Result);
        }


        [TestMethod]
        public void GuestControllerGetAllTest()
        {
            EFWorkUnitMock.Setup(x => x.Categories.GetAll()).Returns(new List<Category>());
            CategoryServiceMock.Setup(x => x.GetAllCategories()).Returns(new List<CategoryDTO>());

            var categoryService = new CategoryService(EFWorkUnitMock.Object);//Возможно это даже не надо создавать, а в expected присваивать CategoryServiceMock.Object.GetAllCategories()

            CategoryController controller = new CategoryController(CategoryServiceMock.Object);


            var result = controller.Get();

            IEnumerable<CategoryModel> expected = mapper.Map<IEnumerable<CategoryDTO>, IEnumerable<CategoryModel>>(CategoryServiceMock.Object.GetAllCategories());
            CollectionAssert.AreEqual(expected.ToList(), result.ToList());
        }


        [TestMethod]
        public void GuestControllerPostTest()
        {
            EFWorkUnitMock.Setup(x => x.Categories.Create(new Category()));
            CategoryServiceMock.Setup(x => x.AddCategory(new CategoryDTO()));

            CategoryController controller = new CategoryController(CategoryServiceMock.Object);

            controller.Post(httpRequest, new CategoryRequest());

            //Похоже суть теста в том, что бы проверить, вызовется ли в контроллере сервис метод с такими же данными ( new GuestDTO() ) 
            CategoryServiceMock.Verify(x => x.AddCategory(new CategoryDTO()));
        }


        [TestMethod]
        public void GuestControllerDeleteTest()
        {
            int CategoryId = 1;
            EFWorkUnitMock.Setup(x => x.Categories.Delete(CategoryId)); //Если вместо Guests сделать Rooms то тест все равно проходит, стра
            CategoryServiceMock.Setup(x => x.DeleteCategory(CategoryId));

            CategoryController controller = new CategoryController(CategoryServiceMock.Object);

            controller.Delete(httpRequest, CategoryId);


            CategoryServiceMock.Verify(x => x.DeleteCategory(CategoryId));
        }

        [TestMethod]
        public void RoomControllerUpdateTest()
        {
            int CategoryId = 1;
            EFWorkUnitMock.Setup(x => x.Categories.Update(new Category()));
            CategoryServiceMock.Setup(x => x.UpdateCategory(new CategoryDTO()));

            CategoryController controller = new CategoryController(CategoryServiceMock.Object);

            controller.Put(CategoryId, httpRequest, new CategoryRequest());

            //Похоже суть теста в том, что бы проверить, вызовется ли в контроллере сервис метод с такими же данными 
            CategoryServiceMock.Verify(x => x.UpdateCategory(new CategoryDTO() { Id = 1 }));
        }
    }
}
