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
    public class RoomContollerTest
    {
        private readonly RoomController roomController;
        IMapper mapper;
        Mock<IRoomService> RoomServiceMock;
        Mock<ICategoryService> CategoryServiceMock;

        HttpConfiguration httpConfiguration;
        HttpRequestMessage httpRequest;

        public RoomContollerTest()
        {
            mapper = new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<Room, RoomDTO>();
                cfg.CreateMap<RoomDTO, RoomModel>();

            }).CreateMapper();

            RoomServiceMock = new Mock<IRoomService>();
            CategoryServiceMock = new Mock<ICategoryService>();

            roomController = new RoomController(RoomServiceMock.Object, CategoryServiceMock.Object);

            httpConfiguration = new HttpConfiguration();
            httpRequest = new System.Net.Http.HttpRequestMessage();
            httpRequest.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = httpConfiguration;
        }

        [TestMethod]
        public void GetByIdIsHttpResponse()
        {
            int RoomId = 1;
            RoomServiceMock.Setup(a => a.Get(RoomId)).Returns(new RoomDTO());

            var httpResponse = roomController.Get(httpRequest, RoomId);


            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void GetByIdHttpResponseIsRoomModel()
        {
            int RoomId = 1;
            RoomServiceMock.Setup(a => a.Get(RoomId)).Returns(new RoomDTO());

            var httpResponse = roomController.Get(httpRequest, RoomId);
            var result = httpResponse.Content.ReadAsAsync<RoomModel>();

            Assert.IsInstanceOfType(result.Result, typeof(RoomModel));
        }


        [TestMethod]
        public void GetById_ShouldReturnRoomModel()
        {
            int RoomId = 1;

            RoomServiceMock.Setup(a => a.Get(RoomId)).Returns(new RoomDTO());

            var httpResponse = roomController.Get(httpRequest, RoomId);
            var result = httpResponse.Content.ReadAsAsync<RoomModel>();
            RoomModel expected = mapper.Map<RoomDTO, RoomModel>(RoomServiceMock.Object.Get(RoomId));

            Assert.AreEqual(expected, result.Result);
        }

        [TestMethod]
        public void GetById_ShouldReturnOK_WhenRoomExsists()
        {
            int RoomId = 11111;

            RoomServiceMock.Setup(a => a.Get(RoomId)).Returns(new RoomDTO());

            var httpResponse = roomController.Get(httpRequest, RoomId);
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.OK, result);
        }


        [TestMethod]
        public void GetById_ShouldReturnNotFound_WhenRoomNotExsists()
        {
            int RoomId = 11111;

            RoomServiceMock.Setup(a => a.Get(RoomId)).Throws(new NullReferenceException());

            var httpResponse = roomController.Get(httpRequest, RoomId);
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.NotFound, result);
        }


        [TestMethod]
        public void GetAllIsHttpResponse()
        {
            RoomServiceMock.Setup(a => a.GetAllRooms()).Returns(new List<RoomDTO>());

            var httpResponse = roomController.Get(httpRequest);

            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void GetAllHttpResponseIsIEnumerableRoomModel()
        {
            RoomServiceMock.Setup(a => a.GetAllRooms()).Returns(new List<RoomDTO>());

            var httpResponse = roomController.Get(httpRequest);
            var result = httpResponse.Content.ReadAsAsync<IEnumerable<RoomModel>>();

            Assert.IsInstanceOfType(result.Result, typeof(IEnumerable<RoomModel>));
        }


        [TestMethod]
        public void GetAll_ShouldReturnAllRooms()
        {
            RoomServiceMock.Setup(x => x.GetAllRooms()).Returns(new List<RoomDTO>());


            var httpResponse = roomController.Get(httpRequest);
            var result = httpResponse.Content.ReadAsAsync<IEnumerable<RoomModel>>();
            IEnumerable<RoomModel> expected = mapper.Map<IEnumerable<RoomDTO>, IEnumerable<RoomModel>>(RoomServiceMock.Object.GetAllRooms());

            CollectionAssert.AreEqual(expected.ToList(), result.Result.ToList());
        }

        [TestMethod]
        public void PostIsHttpResponse()
        {
            RoomServiceMock.Setup(a => a.AddRoom(new RoomDTO()));

            var httpResponse = roomController.Post(httpRequest, new RoomRequest());

            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void Post_ShouldReturnOK()
        {
            RoomServiceMock.Setup(x => x.AddRoom(new RoomDTO()));


            var httpResponse = roomController.Post(httpRequest, new RoomRequest());
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod]
        public void Post_ShouldReturnBadRequest()
        {
            RoomServiceMock.Setup(x => x.AddRoom(new RoomDTO())).Throws(new ArgumentException());

            var httpResponse = roomController.Post(httpRequest, new RoomRequest());
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.BadRequest, result);
        }

        [TestMethod]
        public void Post_ShouldAddRoom()
        {
            RoomServiceMock.Setup(x => x.AddRoom(new RoomDTO()));


            roomController.Post(httpRequest, new RoomRequest());

            RoomServiceMock.Verify(x => x.AddRoom(new RoomDTO()));
        }


        [TestMethod]
        public void DeleteIsHttpResponse()
        {
            var RoomId = 1;
            RoomServiceMock.Setup(a => a.DeleteRoom(RoomId));

            var httpResponse = roomController.Delete(httpRequest, RoomId);

            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void Delete_ShouldReturnOK()
        {
            var RoomId = 1;

            RoomServiceMock.Setup(x => x.DeleteRoom(RoomId));

            var httpResponse = roomController.Delete(httpRequest, RoomId);

            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod]
        public void Delete_ShouldReturnNotFound()
        {
            var RoomId = 1;
            RoomServiceMock.Setup(x => x.DeleteRoom(RoomId)).Throws(new NullReferenceException());


            var httpResponse = roomController.Delete(httpRequest, RoomId);
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.NotFound, result);
        }

        [TestMethod]
        public void Delete_ShouldDeleteRoom()
        {
            int RoomId = 1;

            RoomServiceMock.Setup(x => x.DeleteRoom(RoomId));

            roomController.Delete(httpRequest, RoomId);

            RoomServiceMock.Verify(x => x.DeleteRoom(RoomId));
        }

        [TestMethod]
        public void PutIsHttpResponse()
        {
            int RoomId = 1;
            RoomServiceMock.Setup(a => a.UpdateRoom(new RoomDTO() { Id = RoomId }));

            var httpResponse = roomController.Put(RoomId, httpRequest, new RoomRequest());

            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void Put_ShouldReturnOK()
        {
            int RoomId = 1;
            RoomServiceMock.Setup(a => a.UpdateRoom(new RoomDTO() { Id = RoomId }));

            var httpResponse = roomController.Put(RoomId, httpRequest, new RoomRequest());

            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod]
        public void Put_ShouldReturnNotFound_WhenNullReferenceException()
        {
            int RoomId = 1;

            var roomDTO = new RoomDTO() { Id = RoomId, Name = "101", Active = true };
            RoomServiceMock.Setup(a => a.UpdateRoom(roomDTO)).Throws(new NullReferenceException());

            var httpResponse = roomController.Put(RoomId, httpRequest, new RoomRequest() { Name = "101", Active = true });
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.NotFound, result);
        }

        [TestMethod]
        public void Put_ShouldReturnBadRequest_WhenArgumentException()
        {
            int RoomId = 1;
            RoomServiceMock.Setup(a => a.UpdateRoom(new RoomDTO() { Id = RoomId })).Throws(new ArgumentException());

            var httpResponse = roomController.Put(RoomId, httpRequest, new RoomRequest());
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.BadRequest, result);
        }

        [TestMethod]
        public void Put_ShouldUpdateRoom()
        {
            int RoomId = 1;
            RoomServiceMock.Setup(a => a.UpdateRoom(new RoomDTO() { Id = RoomId }));

            var httpResponse = roomController.Put(RoomId, httpRequest, new RoomRequest());

            RoomServiceMock.Verify(x => x.UpdateRoom(new RoomDTO() { Id = RoomId }));
        }
    }
}
