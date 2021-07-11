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
    public class GuestControllerTest
    {
        IMapper mapper;
        Mock<IWorkUnit> EFWorkUnitMock;
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

            EFWorkUnitMock = new Mock<IWorkUnit>();
            GuestServiceMock = new Mock<IGuestService>();


            httpConfiguration = new HttpConfiguration();
            httpRequest = new System.Net.Http.HttpRequestMessage();
            httpRequest.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = httpConfiguration;
        }

        [TestMethod]
        public void GuestControllerGetTest()
        {
            int GuestId = 1;

            EFWorkUnitMock.Setup(x => x.Guests.Get(GuestId)).Returns(new Guest());
            GuestServiceMock.Setup(a => a.Get(GuestId)).Returns(new GuestDTO());

            var guestService = new GuestService(EFWorkUnitMock.Object);
            GuestController controller = new GuestController(GuestServiceMock.Object);

            var httpResponse = controller.Get(httpRequest, GuestId);
            var result = httpResponse.Content.ReadAsAsync<GuestModel>();
            GuestModel expected = mapper.Map<GuestDTO, GuestModel>(guestService.Get(GuestId));
            Assert.AreEqual(expected, result.Result);
        }


        [TestMethod]
        public void GuestControllerGetAllTest()
        {
            EFWorkUnitMock.Setup(x => x.Guests.GetAll()).Returns(new List<Guest>());
            GuestServiceMock.Setup(x => x.GetAllGuests()).Returns(new List<GuestDTO>());

            var guestService = new GuestService(EFWorkUnitMock.Object);
            GuestController controller = new GuestController(GuestServiceMock.Object);


            var result = controller.Get();

            IEnumerable<GuestModel> expected = mapper.Map<IEnumerable<GuestDTO>, IEnumerable<GuestModel>>(guestService.GetAllGuests());
            CollectionAssert.AreEqual(expected.ToList(), result.ToList());
        }


        [TestMethod]
        public void GuestControllerPostTest()
        {
            EFWorkUnitMock.Setup(x => x.Guests.Create(new Guest()));
            GuestServiceMock.Setup(x => x.AddGuest(new GuestDTO()));

            GuestController controller = new GuestController(GuestServiceMock.Object);

            controller.Post(httpRequest, new GuestRequest());

            GuestServiceMock.Verify(x => x.AddGuest(new GuestDTO()));
        }


        [TestMethod]
        public void GuestControllerDeleteTest()
        {
            int GuestId = 1;
            EFWorkUnitMock.Setup(x => x.Guests.Delete(GuestId)); //Если вместо Guests сделать Rooms то тест все равно проходит, стра
            GuestServiceMock.Setup(x => x.DeleteGuest(GuestId));

            GuestController controller = new GuestController(GuestServiceMock.Object);

            controller.Delete(httpRequest, GuestId);


            GuestServiceMock.Verify(x => x.DeleteGuest(GuestId));
        }

        [TestMethod]
        public void RoomControllerUpdateTest()
        {
            int RoomId = 1;
            EFWorkUnitMock.Setup(x => x.Guests.Update(new Guest()));
            GuestServiceMock.Setup(x => x.UpdateGuest(new GuestDTO()));

            GuestController controller = new GuestController(GuestServiceMock.Object);

            controller.Put(RoomId, httpRequest, new GuestRequest());

            //Похоже суть теста в том, что бы проверить, вызовется ли в контроллере сервис метод с такими же данными 
            GuestServiceMock.Verify(x => x.UpdateGuest(new GuestDTO() { Id = 1 }));
        }

    }
}
