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
using System.Net.Http;
using System.Web.Http;

namespace Hotel.Tests.ControllerTests
{
    [TestClass]
    public class BookingControllerTest
    {
        IMapper mapper;
        Mock<IWorkUnit> EFWorkUnitMock;
        Mock<IBookingService> BookingServiceMock;
        Mock<IGuestService> GuestServiceMock;
        Mock<IRoomService> RoomServiceMock;

        HttpConfiguration httpConfiguration;
        HttpRequestMessage httpRequest;

        public BookingControllerTest()
        {
            mapper = new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<Booking, BookingDTO>();
                cfg.CreateMap<Room, RoomDTO>();
                cfg.CreateMap<Guest, GuestDTO>();
                cfg.CreateMap<BookingDTO, BookingModel>();
            }).CreateMapper();

            EFWorkUnitMock = new Mock<IWorkUnit>();
            RoomServiceMock = new Mock<IRoomService>();
            GuestServiceMock = new Mock<IGuestService>();
            BookingServiceMock = new Mock<IBookingService>();

            httpConfiguration = new HttpConfiguration();
            httpRequest = new System.Net.Http.HttpRequestMessage();
            httpRequest.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = httpConfiguration;
        }


        [TestMethod]
        public void BookingControllerGetTest()
        {
            int BookingId = 1;

            EFWorkUnitMock.Setup(x => x.Bookings.Get(BookingId)).Returns(new Booking());
            BookingServiceMock.Setup(a => a.Get(BookingId)).Returns(new BookingDTO());

            BookingController controller = new BookingController(BookingServiceMock.Object ,RoomServiceMock.Object, GuestServiceMock.Object);

            var httpResponse = controller.Get(httpRequest, BookingId);
            var result = httpResponse.Content.ReadAsAsync<BookingModel>();
            BookingModel expected = mapper.Map<BookingDTO, BookingModel>(BookingServiceMock.Object.Get(BookingId));
            Assert.AreEqual(expected, result.Result);
        }


        [TestMethod]
        public void BookingControllerGetAllTest()
        {
            EFWorkUnitMock.Setup(x => x.Bookings.GetAll()).Returns(new List<Booking>());
            BookingServiceMock.Setup(x => x.GetAllBookings()).Returns(new List<BookingDTO>());

            BookingController controller = new BookingController(BookingServiceMock.Object, RoomServiceMock.Object, GuestServiceMock.Object);


            var result = controller.Get();

            IEnumerable<BookingModel> expected = mapper.Map<IEnumerable<BookingDTO>, IEnumerable<BookingModel>>(BookingServiceMock.Object.GetAllBookings());
            CollectionAssert.AreEqual(expected.ToList(), result.ToList());
        }


        [TestMethod]
        public void BookingControllerPostTest()
        {
            EFWorkUnitMock.Setup(x => x.Bookings.Create(new Booking()));
            BookingServiceMock.Setup(x => x.AddBooking(new BookingDTO()));

            BookingController controller = new BookingController(BookingServiceMock.Object, RoomServiceMock.Object, GuestServiceMock.Object);

            controller.Post(httpRequest, new BookingRequest());

            //Похоже суть теста в том, что бы проверить, вызовется ли в контроллере сервис метод с такими же данными ( new RoomDTO() ) 
            BookingServiceMock.Verify(x => x.AddBooking(new BookingDTO()));
        }


        [TestMethod]
        public void BookingControllerDeleteTest()
        {
            int BookingId = 1;
            EFWorkUnitMock.Setup(x => x.Bookings.Delete(BookingId));
            BookingServiceMock.Setup(x => x.DeleteBooking(BookingId));

            BookingController controller = new BookingController(BookingServiceMock.Object, RoomServiceMock.Object, GuestServiceMock.Object);

            controller.Delete(httpRequest, BookingId);


            BookingServiceMock.Verify(x => x.DeleteBooking(BookingId));
        }

        [TestMethod]
        public void BookingontrollerUpdateTest()
        {
            int BookingId = 1;
            EFWorkUnitMock.Setup(x => x.Bookings.Update(new Booking()));
            BookingServiceMock.Setup(x => x.UpdateBooking(new BookingDTO()));

            BookingController controller = new BookingController(BookingServiceMock.Object, RoomServiceMock.Object, GuestServiceMock.Object);

            controller.Put(httpRequest , BookingId, new BookingRequest());       

            //Похоже суть теста в том, что бы проверить, вызовется ли в контроллере сервис метод с такими же данными 
            BookingServiceMock.Verify(x => x.UpdateBooking(new BookingDTO() { Id = 1 }));
        }
    }
}
