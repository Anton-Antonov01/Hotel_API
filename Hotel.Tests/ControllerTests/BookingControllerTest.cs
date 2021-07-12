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
    public class BookingControllerTest
    {
        private readonly BookingController bookingController;
        IMapper mapper;
        Mock<IBookingService> BookingServiceMock;
        Mock<IRoomService> RoomServiceMock;
        Mock<IGuestService> GuestServideMock;

        HttpConfiguration httpConfiguration;
        HttpRequestMessage httpRequest;

        public BookingControllerTest()
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
        public void GetByIdIsHttpResponse()
        {
            int BookingId = 1;
            BookingServiceMock.Setup(a => a.Get(BookingId)).Returns(new BookingDTO());

            var httpResponse = bookingController.Get(httpRequest, BookingId);


            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void GetByIdHttpResponseIsGuestModel()
        {
            int BookingId = 1;
            BookingServiceMock.Setup(a => a.Get(BookingId)).Returns(new BookingDTO());

            var httpResponse = bookingController.Get(httpRequest, BookingId);
            var result = httpResponse.Content.ReadAsAsync<BookingModel>();

            Assert.IsInstanceOfType(result.Result, typeof(BookingModel));
        }


        [TestMethod]
        public void GetById_ShouldReturnBookingModel()
        {
            int BookingId = 1;

            BookingServiceMock.Setup(a => a.Get(BookingId)).Returns(new BookingDTO());

            var httpResponse = bookingController.Get(httpRequest, BookingId);
            var result = httpResponse.Content.ReadAsAsync<BookingModel>();
            BookingModel expected = mapper.Map<BookingDTO, BookingModel>(BookingServiceMock.Object.Get(BookingId));

            Assert.AreEqual(expected, result.Result);
        }

        [TestMethod]
        public void GetById_ShouldReturnOK_WhenBookingExsists()
        {
            int BookingId = 11111;

            BookingServiceMock.Setup(a => a.Get(BookingId)).Returns(new BookingDTO());

            var httpResponse = bookingController.Get(httpRequest, BookingId);
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.OK, result);
        }


        [TestMethod]
        public void GetById_ShouldReturnNotFound_WhenBookingNotExsists()
        {
            int BookingId = 11111;

            BookingServiceMock.Setup(a => a.Get(BookingId)).Throws(new NullReferenceException());

            var httpResponse = bookingController.Get(httpRequest, BookingId);
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.NotFound, result);
        }


        [TestMethod]
        public void GetAllIsHttpResponse()
        {
            BookingServiceMock.Setup(a => a.GetAllBookings()).Returns(new List<BookingDTO>());

            var httpResponse = bookingController.Get(httpRequest);

            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void GetAllHttpResponseIsIEnumerableBookingModel()
        {
            BookingServiceMock.Setup(a => a.GetAllBookings()).Returns(new List<BookingDTO>());

            var httpResponse = bookingController.Get(httpRequest);
            var result = httpResponse.Content.ReadAsAsync<IEnumerable<BookingModel>>();

            Assert.IsInstanceOfType(result.Result, typeof(IEnumerable<BookingModel>));
        }


        [TestMethod]
        public void GetAll_ShouldReturnAllBookings()
        {
            BookingServiceMock.Setup(x => x.GetAllBookings()).Returns(new List<BookingDTO>());


            var httpResponse = bookingController.Get(httpRequest);
            var result = httpResponse.Content.ReadAsAsync<IEnumerable<BookingModel>>();
            IEnumerable<BookingModel> expected = mapper.Map<IEnumerable<BookingDTO>, IEnumerable<BookingModel>>(BookingServiceMock.Object.GetAllBookings());

            CollectionAssert.AreEqual(expected.ToList(), result.Result.ToList());
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


        [TestMethod]
        public void DeleteIsHttpResponse()
        {
            var BookingId = 1;
            BookingServiceMock.Setup(a => a.DeleteBooking(BookingId));

            var httpResponse = bookingController.Delete(httpRequest, BookingId);

            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void Delete_ShouldReturnOK()
        {
            var BookingId = 1;

            BookingServiceMock.Setup(x => x.DeleteBooking(BookingId));

            var httpResponse = bookingController.Delete(httpRequest, BookingId);

            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod]
        public void Delete_ShouldReturnNotFound()
        {
            var BookingId = 1;
            BookingServiceMock.Setup(x => x.DeleteBooking(BookingId)).Throws(new NullReferenceException());


            var httpResponse = bookingController.Delete(httpRequest, BookingId);
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.NotFound, result);
        }

        [TestMethod]
        public void Delete_ShouldDeleteBooking()
        {
            int BookingId = 1;

            BookingServiceMock.Setup(x => x.DeleteBooking(BookingId));

            bookingController.Delete(httpRequest, BookingId);

            BookingServiceMock.Verify(x => x.DeleteBooking(BookingId));
        }

        [TestMethod]
        public void PutIsHttpResponse()
        {
            int BookingId = 1;
            BookingServiceMock.Setup(a => a.UpdateBooking(new BookingDTO() { Id = BookingId }));

            var httpResponse = bookingController.Put(httpRequest, BookingId, new BookingRequest());

            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void Put_ShouldReturnOK()
        {
            int BookingId = 1;
            BookingServiceMock.Setup(a => a.UpdateBooking(new BookingDTO() { Id = BookingId }));

            var httpResponse = bookingController.Put(httpRequest, BookingId, new BookingRequest());

            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod]
        public void Put_ShouldReturnNotFound_WhenNullReferenceException()
        {
            int BookingId = 1;

            BookingServiceMock.Setup(a => a.UpdateBooking(new BookingDTO() { Id = BookingId })).Throws(new NullReferenceException());

            var httpResponse = bookingController.Put(httpRequest, BookingId, new BookingRequest() {});
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.NotFound, result);
        }

        [TestMethod]
        public void Put_ShouldReturnBadRequest_WhenArgumentException()
        {
            int BookingId = 1;
            BookingServiceMock.Setup(a => a.UpdateBooking(new BookingDTO() { Id = BookingId })).Throws(new ArgumentException());

            var httpResponse = bookingController.Put(httpRequest, BookingId, new BookingRequest() { });
            var result = httpResponse.StatusCode;

            Assert.AreEqual(HttpStatusCode.BadRequest, result);
        }

        [TestMethod]
        public void Put_ShouldUpdateBooking()
        {
            int BookingId = 1;
            BookingServiceMock.Setup(a => a.UpdateBooking(new BookingDTO() { Id = BookingId }));

            var httpResponse = bookingController.Put(httpRequest, BookingId, new BookingRequest() { });

            BookingServiceMock.Verify(x => x.UpdateBooking(new BookingDTO() { Id = BookingId }));
        }
    }
}
