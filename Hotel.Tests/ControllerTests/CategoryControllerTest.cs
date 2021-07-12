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
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Hotel.Tests.ControllerTests
{
    [TestClass]
    public class CategoryControllerTest
    {
        private readonly CategoryController categoryController;
        IMapper mapper;
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

            CategoryServiceMock = new Mock<ICategoryService>();
            categoryController = new CategoryController(CategoryServiceMock.Object);

            httpConfiguration = new HttpConfiguration();
            httpRequest = new System.Net.Http.HttpRequestMessage();
            httpRequest.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = httpConfiguration;
        }

        [TestMethod]
        public void GetByIdIsHttpResponse()
        {
            int CategoryId = 1;
            CategoryServiceMock.Setup(a => a.Get(CategoryId)).Returns(new CategoryDTO());

            var httpResponse = categoryController.Get(httpRequest, CategoryId);


            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void GetByIdHttpResponseIsCategoryModel()
        {
            int CategoryId = 1;
            CategoryServiceMock.Setup(a => a.Get(CategoryId)).Returns(new CategoryDTO());

            var httpResponse = categoryController.Get(httpRequest, CategoryId);
            var result = httpResponse.Content.ReadAsAsync<CategoryModel>();

            Assert.IsInstanceOfType(result.Result, typeof(CategoryModel));
        }


        [TestMethod]
        public void GetById_ShouldReturnCategoryModel()
        {
            int CategoryId = 1;

            CategoryServiceMock.Setup(a => a.Get(CategoryId)).Returns(new CategoryDTO());

            var httpResponse = categoryController.Get(httpRequest, CategoryId);
            var result = httpResponse.Content.ReadAsAsync<CategoryModel>();
            CategoryModel expected = mapper.Map<CategoryDTO, CategoryModel>(CategoryServiceMock.Object.Get(CategoryId));

            Assert.AreEqual(expected, result.Result);
        }

        [TestMethod]
        public void GetById_ShouldReturnOK_WhenCategoryExsists()
        {
            int CategoryId = 11111;

            CategoryServiceMock.Setup(a => a.Get(CategoryId)).Returns(new CategoryDTO());

            var httpResponse = categoryController.Get(httpRequest, CategoryId);
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.OK, result);
        }


        [TestMethod]
        public void GetById_ShouldReturnNotFound_WhenCategoryNotExsists()
        {
            int CategoryId = 11111;

            CategoryServiceMock.Setup(a => a.Get(CategoryId)).Throws(new NullReferenceException());

            var httpResponse = categoryController.Get(httpRequest, CategoryId);
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.NotFound, result);
        }


        [TestMethod]
        public void GetAllIsHttpResponse()
        {
            CategoryServiceMock.Setup(a => a.GetAllCategories()).Returns(new List<CategoryDTO>());

            var httpResponse = categoryController.Get(httpRequest);

            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void GetAllHttpResponseIsIEnumerableCategoryModel()
        {
            CategoryServiceMock.Setup(a => a.GetAllCategories()).Returns(new List<CategoryDTO>());

            var httpResponse = categoryController.Get(httpRequest);
            var result = httpResponse.Content.ReadAsAsync<IEnumerable<CategoryModel>>();

            Assert.IsInstanceOfType(result.Result, typeof(IEnumerable<CategoryModel>));
        }


        [TestMethod]
        public void GetAll_ShouldReturnAllCategory()
        {
            CategoryServiceMock.Setup(x => x.GetAllCategories()).Returns(new List<CategoryDTO>());


            var httpResponse = categoryController.Get(httpRequest);
            var result = httpResponse.Content.ReadAsAsync<IEnumerable<CategoryModel>>();
            IEnumerable<CategoryModel> expected = mapper.Map<IEnumerable<CategoryDTO>, IEnumerable<CategoryModel>>(CategoryServiceMock.Object.GetAllCategories());

            CollectionAssert.AreEqual(expected.ToList(), result.Result.ToList());
        }

        [TestMethod]
        public void PostIsHttpResponse()
        {
            CategoryServiceMock.Setup(a => a.AddCategory(new CategoryDTO()));

            var httpResponse = categoryController.Post(httpRequest, new CategoryRequest());

            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void Post_ShouldReturnOK()
        {
            CategoryServiceMock.Setup(x => x.AddCategory(new CategoryDTO()));


            var httpResponse = categoryController.Post(httpRequest, new CategoryRequest());
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod]
        public void Post_ShouldReturnBadRequest()
        {
            CategoryServiceMock.Setup(x => x.AddCategory(new CategoryDTO())).Throws(new ArgumentException());

            var httpResponse = categoryController.Post(httpRequest, new CategoryRequest());
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.BadRequest, result);
        }

        [TestMethod]
        public void Post_ShouldAddCategory()
        {
            CategoryServiceMock.Setup(x => x.AddCategory(new CategoryDTO()));


            categoryController.Post(httpRequest, new CategoryRequest());

            CategoryServiceMock.Verify(x => x.AddCategory(new CategoryDTO()));
        }


        [TestMethod]
        public void DeleteIsHttpResponse()
        {
            var CategoryId = 1;
            CategoryServiceMock.Setup(a => a.DeleteCategory(CategoryId));

            var httpResponse = categoryController.Delete(httpRequest, CategoryId);

            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void Delete_ShouldReturnOK()
        {
            var CategoryId = 1;

            CategoryServiceMock.Setup(x => x.DeleteCategory(CategoryId));

            var httpResponse = categoryController.Delete(httpRequest, CategoryId);

            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod]
        public void Delete_ShouldReturnNotFound()
        {
            var CategoryId = 1;
            CategoryServiceMock.Setup(x => x.DeleteCategory(CategoryId)).Throws(new NullReferenceException());


            var httpResponse = categoryController.Delete(httpRequest, CategoryId);
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.NotFound, result);
        }

        [TestMethod]
        public void Delete_ShouldDeleteCategory()
        {
            int CategoryId = 1;

            CategoryServiceMock.Setup(x => x.DeleteCategory(CategoryId));

            categoryController.Delete(httpRequest, CategoryId);

            CategoryServiceMock.Verify(x => x.DeleteCategory(CategoryId));
        }

        [TestMethod]
        public void PutIsHttpResponse()
        {
            int CategoryId = 1;
            CategoryServiceMock.Setup(a => a.UpdateCategory(new CategoryDTO() { Id = CategoryId }));

            var httpResponse = categoryController.Put(CategoryId, httpRequest, new CategoryRequest());

            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void Put_ShouldReturnOK()
        {
            int CategoryId = 1;
            CategoryServiceMock.Setup(a => a.UpdateCategory(new CategoryDTO() { Id = CategoryId }));

            var httpResponse = categoryController.Put(CategoryId, httpRequest, new CategoryRequest());

            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod]
        public void Put_ShouldReturnNotFound_WhenNullReferenceException()
        {
            int CategoryId = 1;
            CategoryServiceMock.Setup(a => a.UpdateCategory(new CategoryDTO() { Id = CategoryId })).Throws(new NullReferenceException());

            var httpResponse = categoryController.Put(CategoryId, httpRequest, new CategoryRequest());
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.NotFound, result);
        }

        [TestMethod]
        public void Put_ShouldReturnBadRequest_WhenArgumentException()
        {
            int CategoryId = 1;
            CategoryServiceMock.Setup(a => a.UpdateCategory(new CategoryDTO() { Id = CategoryId })).Throws(new ArgumentException());

            var httpResponse = categoryController.Put(CategoryId, httpRequest, new CategoryRequest());
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.BadRequest, result);
        }

        [TestMethod]
        public void Put_ShouldUpdateCategory()
        {
            int CategoryId = 1;
            CategoryServiceMock.Setup(a => a.UpdateCategory(new CategoryDTO() { Id = CategoryId }));

            var httpResponse = categoryController.Put(CategoryId, httpRequest, new CategoryRequest());

            CategoryServiceMock.Verify(x => x.UpdateCategory(new CategoryDTO() { Id = CategoryId }));
        }
    }
}
