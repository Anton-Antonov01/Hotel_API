using AutoMapper;
using Hotel_API.Controllers;
using Hotel_API.Models;
using Hotel_API.RequestModels;
using Hotel_BL.DTO;
using Hotel_BL.Interfaces;
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
    public class PriceCategoryControllerTest
    {
        private readonly PriceCategoryController priceCategoryController;
        IMapper mapper;
        Mock<IPriceCategoryService> PriceCategoryServiceMock;
        Mock<ICategoryService> CategoryServiceMock;


        HttpConfiguration httpConfiguration;
        HttpRequestMessage httpRequest;

        public PriceCategoryControllerTest()
        {
            mapper = new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<PriceCategory, PriceCategoryDTO>();
                cfg.CreateMap<PriceCategoryDTO, PriceCategoryModel>();

            }).CreateMapper();

            PriceCategoryServiceMock = new Mock<IPriceCategoryService>();
            CategoryServiceMock = new Mock<ICategoryService>();

            priceCategoryController = new PriceCategoryController(PriceCategoryServiceMock.Object, CategoryServiceMock.Object);

            httpConfiguration = new HttpConfiguration();
            httpRequest = new HttpRequestMessage();
            httpRequest.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = httpConfiguration;
        }

        [TestMethod]
        public void GetByIdIsHttpResponse()
        {
            int PriceCategoryId = 1;
            PriceCategoryServiceMock.Setup(a => a.Get(PriceCategoryId)).Returns(new PriceCategoryDTO());

            var httpResponse = priceCategoryController.Get(httpRequest, PriceCategoryId);


            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void GetByIdHttpResponseIsGuestModel()
        {
            int PriceCategoryId = 1;
            PriceCategoryServiceMock.Setup(a => a.Get(PriceCategoryId)).Returns(new PriceCategoryDTO());

            var httpResponse = priceCategoryController.Get(httpRequest, PriceCategoryId);
            var result = httpResponse.Content.ReadAsAsync<PriceCategoryModel>();

            Assert.IsInstanceOfType(result.Result, typeof(PriceCategoryModel));
        }


        [TestMethod]
        public void GetById_ShouldReturnPriceCategoryModel()
        {
            int PriceCategoryId = 1;

            PriceCategoryServiceMock.Setup(a => a.Get(PriceCategoryId)).Returns(new PriceCategoryDTO());

            var httpResponse = priceCategoryController.Get(httpRequest, PriceCategoryId);
            var result = httpResponse.Content.ReadAsAsync<PriceCategoryModel>();
            PriceCategoryModel expected = mapper.Map<PriceCategoryDTO, PriceCategoryModel>(PriceCategoryServiceMock.Object.Get(PriceCategoryId));

            Assert.AreEqual(expected, result.Result);
        }

        [TestMethod]
        public void GetById_ShouldReturnOK_WhenPriceCategoryExsists()
        {
            int PriceCategoryId = 11111;

            PriceCategoryServiceMock.Setup(a => a.Get(PriceCategoryId)).Returns(new PriceCategoryDTO());

            var httpResponse = priceCategoryController.Get(httpRequest, PriceCategoryId);
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.OK, result);
        }


        [TestMethod]
        public void GetById_ShouldReturnNotFound_WhenPriceCategoryNotExsists()
        {
            int PriceCategoryId = 11111;

            PriceCategoryServiceMock.Setup(a => a.Get(PriceCategoryId)).Throws(new NullReferenceException());

            var httpResponse = priceCategoryController.Get(httpRequest, PriceCategoryId);
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.NotFound, result);
        }


        [TestMethod]
        public void GetAllIsHttpResponse()
        {
            PriceCategoryServiceMock.Setup(a => a.GetAllPriceCategories()).Returns(new List<PriceCategoryDTO>());

            var httpResponse = priceCategoryController.Get(httpRequest);

            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void GetAllHttpResponseIsIEnumerablePriceCategoryModel()
        {
            PriceCategoryServiceMock.Setup(a => a.GetAllPriceCategories()).Returns(new List<PriceCategoryDTO>());

            var httpResponse = priceCategoryController.Get(httpRequest);
            var result = httpResponse.Content.ReadAsAsync<IEnumerable<PriceCategoryModel>>();

            Assert.IsInstanceOfType(result.Result, typeof(IEnumerable<PriceCategoryModel>));
        }


        [TestMethod]
        public void GetAll_ShouldReturnAllPriceCategories()
        {
            PriceCategoryServiceMock.Setup(x => x.GetAllPriceCategories()).Returns(new List<PriceCategoryDTO>());


            var httpResponse = priceCategoryController.Get(httpRequest);
            var result = httpResponse.Content.ReadAsAsync<IEnumerable<PriceCategoryModel>>();
            IEnumerable<PriceCategoryModel> expected = mapper.Map<IEnumerable<PriceCategoryDTO>, IEnumerable<PriceCategoryModel>>(PriceCategoryServiceMock.Object.GetAllPriceCategories());

            CollectionAssert.AreEqual(expected.ToList(), result.Result.ToList());
        }

        [TestMethod]
        public void PostIsHttpResponse()
        {
            PriceCategoryServiceMock.Setup(a => a.AddPriceCategory(new PriceCategoryDTO()));

            var httpResponse = priceCategoryController.Post(httpRequest, new PriceCategoryRequest());

            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void Post_ShouldReturnOK()
        {
            PriceCategoryServiceMock.Setup(x => x.AddPriceCategory(new PriceCategoryDTO()));


            var httpResponse = priceCategoryController.Post(httpRequest, new PriceCategoryRequest());
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod]
        public void Post_ShouldReturnBadRequest()
        {
            PriceCategoryServiceMock.Setup(x => x.AddPriceCategory(new PriceCategoryDTO())).Throws(new ArgumentException());

            var httpResponse = priceCategoryController.Post(httpRequest, new PriceCategoryRequest());
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.BadRequest, result);
        }

        [TestMethod]
        public void Post_ShouldAddPriceCategory()
        {
            PriceCategoryServiceMock.Setup(x => x.AddPriceCategory(new PriceCategoryDTO()));


            priceCategoryController.Post(httpRequest, new PriceCategoryRequest());

            PriceCategoryServiceMock.Verify(x => x.AddPriceCategory(new PriceCategoryDTO()));
        }


        [TestMethod]
        public void DeleteIsHttpResponse()
        {
            var PriceCategoryId = 1;
            PriceCategoryServiceMock.Setup(a => a.DeletePriceCategory(PriceCategoryId));

            var httpResponse = priceCategoryController.Delete(httpRequest, PriceCategoryId);

            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void Delete_ShouldReturnOK()
        {
            var PriceCategoryId = 1;

            PriceCategoryServiceMock.Setup(x => x.DeletePriceCategory(PriceCategoryId));

            var httpResponse = priceCategoryController.Delete(httpRequest, PriceCategoryId);

            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod]
        public void Delete_ShouldReturnNotFound()
        {
            var PriceCategoryId = 1;
            PriceCategoryServiceMock.Setup(x => x.DeletePriceCategory(PriceCategoryId)).Throws(new NullReferenceException());


            var httpResponse = priceCategoryController.Delete(httpRequest, PriceCategoryId);
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.NotFound, result);
        }

        [TestMethod]
        public void Delete_ShouldDeletePriceCategory()
        {
            int PriceCategoryId = 1;

            PriceCategoryServiceMock.Setup(x => x.DeletePriceCategory(PriceCategoryId));

            priceCategoryController.Delete(httpRequest, PriceCategoryId);

            PriceCategoryServiceMock.Verify(x => x.DeletePriceCategory(PriceCategoryId));
        }

        [TestMethod]
        public void PutIsHttpResponse()
        {
            int PriceCategoryId = 1;
            PriceCategoryServiceMock.Setup(a => a.UpdatePriceCategory(new PriceCategoryDTO() { Id = PriceCategoryId }));

            var httpResponse = priceCategoryController.Put(httpRequest, PriceCategoryId, new PriceCategoryRequest());

            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void Put_ShouldReturnOK()
        {
            int PriceCategoryId = 1;
            PriceCategoryServiceMock.Setup(a => a.UpdatePriceCategory(new PriceCategoryDTO() { Id = PriceCategoryId }));

            var httpResponse = priceCategoryController.Put(httpRequest, PriceCategoryId, new PriceCategoryRequest());

            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod]
        public void Put_ShouldReturnNotFound_WhenNullReferenceException()
        {
            int PriceCategoryId = 1;

            PriceCategoryServiceMock.Setup(a => a.UpdatePriceCategory(new PriceCategoryDTO() { Id = PriceCategoryId, EndDate = default,  StartDate = default})).Throws(new NullReferenceException());

            var httpResponse = priceCategoryController.Put(httpRequest, PriceCategoryId, new PriceCategoryRequest() {  EndDate = default, StartDate = default });
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.NotFound, result);
        }

        [TestMethod]
        public void Put_ShouldReturnBadRequest_WhenArgumentException()
        {
            int PriceCategoryId = 1;
            PriceCategoryServiceMock.Setup(a => a.UpdatePriceCategory(new PriceCategoryDTO() { Id = PriceCategoryId })).Throws(new ArgumentException());

            var httpResponse = priceCategoryController.Put(httpRequest, PriceCategoryId, new PriceCategoryRequest() { });
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.BadRequest, result);
        }

        [TestMethod]
        public void Put_ShouldUpdatePriceCategory()
        {
            int PriceCategoryId = 1;
            PriceCategoryServiceMock.Setup(a => a.UpdatePriceCategory(new PriceCategoryDTO() { Id = PriceCategoryId }));

            var httpResponse = priceCategoryController.Put(httpRequest, PriceCategoryId, new PriceCategoryRequest() { });

            PriceCategoryServiceMock.Verify(x => x.UpdatePriceCategory(new PriceCategoryDTO() { Id = PriceCategoryId }));
        }
    }
}
