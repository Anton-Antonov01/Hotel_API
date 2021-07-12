using AutoMapper;
using Hotel_API.Controllers;
using Hotel_API.Models;
using Hotel_API.RequestModels;
using Hotel_BL.DTO;
using Hotel_BL.Interfaces;
using Hotel_DAL.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Hotel.Tests.ControllerTests
{
    [TestClass]
    public class RoomBookingControllerTest
    {

        private readonly BookingController bookingController;
        IMapper mapper;
        Mock<IBookingService> BookingServiceMock;
        Mock<IRoomService> RoomServiceMock;
        Mock<IGuestService> GuestServideMock;

        HttpConfiguration httpConfiguration;
        HttpRequestMessage httpRequest;

        public RoomBookingControllerTest()
        {
            mapper = new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<Booking, BookingDTO>();
                cfg.CreateMap<BookingDTO, BookingModel>();

            }).CreateMapper();

            BookingServiceMock = new Mock<IBookingService>();
            RoomServiceMock = new Mock<IRoomService>();
            GuestServideMock = new Mock<IGuestService>();

            bookingController = new BookingController(BookingServiceMock.Object, RoomServiceMock.Object, GuestServideMock.Object);

            httpConfiguration = new HttpConfiguration();
            httpRequest = new HttpRequestMessage();
            httpRequest.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = httpConfiguration;
        }




        [TestMethod]
        public void PostIsHttpResponse()
        {
            BookingServiceMock.Setup(a => a.AddBooking(new BookingDTO()));

            var httpResponse = bookingController.Post(httpRequest, new BookingRequest());

            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void Post_ShouldReturnOK()
        {
            BookingServiceMock.Setup(x => x.AddBooking(new BookingDTO()));


            var httpResponse = bookingController.Post(httpRequest, new BookingRequest());
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod]
        public void Post_ShouldReturnBadRequest()
        {
            BookingServiceMock.Setup(x => x.AddBooking(new BookingDTO())).Throws(new ArgumentException());

            var httpResponse = bookingController.Post(httpRequest, new BookingRequest());
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.BadRequest, result);
        }

        [TestMethod]
        public void Post_ShouldAddBooking()
        {
            BookingServiceMock.Setup(x => x.AddBooking(new BookingDTO()));


            bookingController.Post(httpRequest, new BookingRequest());

            BookingServiceMock.Verify(x => x.AddBooking(new BookingDTO()));
        }
    }
}
