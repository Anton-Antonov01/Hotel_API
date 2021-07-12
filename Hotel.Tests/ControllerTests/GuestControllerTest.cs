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
    public class GuestControllerTest
    {
        private readonly GuestController guestController;
        IMapper mapper;
        Mock<IGuestService> GuestServiceMock;

        HttpConfiguration httpConfiguration;
        HttpRequestMessage httpRequest;

        public GuestControllerTest()
        {
            mapper = new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<Guest, GuestDTO>();
                cfg.CreateMap<GuestDTO, GuestModel>();
            }).CreateMapper();

            GuestServiceMock = new Mock<IGuestService>();
            guestController = new GuestController(GuestServiceMock.Object);

            httpConfiguration = new HttpConfiguration();
            httpRequest = new System.Net.Http.HttpRequestMessage();
            httpRequest.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = httpConfiguration;
        }

        [TestMethod]
        public void GetByIdIsHttpResponse()
        {
            int GuestId = 1;
            GuestServiceMock.Setup(a => a.Get(GuestId)).Returns(new GuestDTO());

            var httpResponse = guestController.Get(httpRequest, GuestId);


            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void GetByIdHttpResponseIsGuestModel()
        {
            int GuestId = 1;
            GuestServiceMock.Setup(a => a.Get(GuestId)).Returns(new GuestDTO());

            var httpResponse = guestController.Get(httpRequest, GuestId);
            var result = httpResponse.Content.ReadAsAsync<GuestModel>();

            Assert.IsInstanceOfType(result.Result, typeof(GuestModel));
        }


        [TestMethod]
        public void GetById_ShouldReturnGuestModel()
        {
            int GuestId = 1;

            GuestServiceMock.Setup(a => a.Get(GuestId)).Returns(new GuestDTO());

            var httpResponse = guestController.Get(httpRequest, GuestId);
            var result = httpResponse.Content.ReadAsAsync<GuestModel>();
            GuestModel expected = mapper.Map<GuestDTO, GuestModel>(GuestServiceMock.Object.Get(GuestId));

            Assert.AreEqual(expected, result.Result);
        }

        [TestMethod]
        public void GetById_ShouldReturnOK_WhenGuestExsists()
        {
            int GuestId = 11111;

            GuestServiceMock.Setup(a => a.Get(GuestId)).Returns(new GuestDTO());

            var httpResponse = guestController.Get(httpRequest, GuestId);
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.OK, result);
        }


        [TestMethod]
        public void GetById_ShouldReturnNotFound_WhenGuestNotExsists()
        {
            int GuestId = 11111;

            GuestServiceMock.Setup(a => a.Get(GuestId)).Throws(new NullReferenceException());  

            var httpResponse = guestController.Get(httpRequest, GuestId);
            var result = httpResponse.StatusCode;
            
            Assert.AreEqual(HttpStatusCode.NotFound, result);
        }


        [TestMethod]
        public void GetAllIsHttpResponse()
        {
            GuestServiceMock.Setup(a => a.GetAllGuests()).Returns(new List<GuestDTO>());

            var httpResponse = guestController.Get(httpRequest);

            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void GetAllHttpResponseIsIEnumerableGuestModel()
        {
            GuestServiceMock.Setup(a => a.GetAllGuests()).Returns(new List<GuestDTO>());

            var httpResponse = guestController.Get(httpRequest);
            var result = httpResponse.Content.ReadAsAsync<IEnumerable<GuestModel>>();

            Assert.IsInstanceOfType(result.Result, typeof(IEnumerable<GuestModel>));
        }


        [TestMethod]
        public void GetAll_ShouldReturnAllGuests()
        {
            GuestServiceMock.Setup(x => x.GetAllGuests()).Returns(new List<GuestDTO>());


            var httpResponse = guestController.Get(httpRequest);
            var result = httpResponse.Content.ReadAsAsync<IEnumerable<GuestModel>>();
            IEnumerable<GuestModel> expected = mapper.Map<IEnumerable<GuestDTO>, IEnumerable<GuestModel>>(GuestServiceMock.Object.GetAllGuests());

            CollectionAssert.AreEqual(expected.ToList(), result.Result.ToList());
        }

        [TestMethod]
        public void PostIsHttpResponse()
        {
            GuestServiceMock.Setup(a => a.AddGuest(new GuestDTO()));

            var httpResponse = guestController.Post(httpRequest, new GuestRequest() );

            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void Post_ShouldReturnOK()
        {
            GuestServiceMock.Setup(x => x.AddGuest(new GuestDTO()));


            var httpResponse = guestController.Post(httpRequest,new GuestRequest());
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod]
        public void Post_ShouldReturnBadRequest()
        {
            GuestServiceMock.Setup(x => x.AddGuest(new GuestDTO())).Throws(new ArgumentException());

            var httpResponse = guestController.Post(httpRequest, new GuestRequest());
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.BadRequest, result);
        }

        [TestMethod]
        public void Post_ShouldAddGuest()
        {
            GuestServiceMock.Setup(x => x.AddGuest(new GuestDTO()));


            guestController.Post(httpRequest, new GuestRequest());

            GuestServiceMock.Verify(x => x.AddGuest(new GuestDTO()));
        }


        [TestMethod]
        public void DeleteIsHttpResponse()
        {
            var GuestId = 1;
            GuestServiceMock.Setup(a => a.DeleteGuest(GuestId));

            var httpResponse = guestController.Delete(httpRequest, GuestId);

            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void Delete_ShouldReturnOK()
        {
            var GuestId = 1;

            GuestServiceMock.Setup(x => x.DeleteGuest(GuestId));

            var httpResponse = guestController.Delete(httpRequest, GuestId);

            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod]
        public void Delete_ShouldReturnNotFound()
        {
            var GuestId = 1;
            GuestServiceMock.Setup(x => x.DeleteGuest(GuestId)).Throws(new NullReferenceException());


            var httpResponse = guestController.Delete(httpRequest, GuestId);
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.NotFound, result);
        }

        [TestMethod]
        public void Delete_ShouldDeleteGuest()
        {
            int GuestId = 1;

            GuestServiceMock.Setup(x => x.DeleteGuest(GuestId));

            guestController.Delete(httpRequest, GuestId);

            GuestServiceMock.Verify(x => x.DeleteGuest(GuestId));
        }

        [TestMethod]
        public void PutIsHttpResponse()
        {
            int GuestId = 1;
            GuestServiceMock.Setup(a => a.UpdateGuest(new GuestDTO() { Id = GuestId }));

            var httpResponse = guestController.Put(GuestId ,httpRequest, new GuestRequest());

            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void Put_ShouldReturnOK()
        {
            int GuestId = 1;
            GuestServiceMock.Setup(a => a.UpdateGuest(new GuestDTO() { Id = GuestId }));

            var httpResponse = guestController.Put(GuestId, httpRequest, new GuestRequest());

            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod]
        public void Put_ShouldReturnNotFound_WhenNullReferenceException()
        {
            int GuestId = 1;
            GuestServiceMock.Setup(a => a.UpdateGuest(new GuestDTO() { Id = GuestId})).Throws(new NullReferenceException());          

            var httpResponse = guestController.Put(GuestId, httpRequest, new GuestRequest());
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.NotFound, result);
        }

        [TestMethod]
        public void Put_ShouldReturnBadRequest_WhenArgumentException()
        {
            int GuestId = 1;
            GuestServiceMock.Setup(a => a.UpdateGuest(new GuestDTO() { Id = GuestId })).Throws(new ArgumentException());

            var httpResponse = guestController.Put(GuestId, httpRequest, new GuestRequest());
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.BadRequest, result);
        }

        [TestMethod]
        public void Put_ShouldUpdateGuest()
        {
            int GuestId = 1;
            GuestServiceMock.Setup(a => a.UpdateGuest(new GuestDTO() { Id = GuestId }));

            var httpResponse = guestController.Put(GuestId, httpRequest, new GuestRequest());

            GuestServiceMock.Verify(x => x.UpdateGuest(new GuestDTO() { Id = GuestId }));
        }

    }
}
