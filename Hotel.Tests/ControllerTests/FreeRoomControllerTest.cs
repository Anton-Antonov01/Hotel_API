using AutoMapper;
using Hotel_API.Controllers;
using Hotel_API.Models;
using Hotel_BL.DTO;
using Hotel_BL.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Hotel.Tests.ControllerTests
{
    [TestClass]
    public class FreeRoomControllerTest
    {
        private readonly FreeRoomsController freeRoomsController;
        Mock<IBookingService> BookingServiceMock;

        public FreeRoomControllerTest()
        {
            BookingServiceMock = new Mock<IBookingService>();
            freeRoomsController = new FreeRoomsController(BookingServiceMock.Object);

        }


        [TestMethod]
        public void FreeRoomsByDateIsIEnumerableRoomModel()
        {
            var date = new DateTime(2021, 3, 15);
            BookingServiceMock.Setup(a => a.FreeRoomsByDate(date)).Returns(new List<RoomDTO>());

            var result = freeRoomsController.FreeRoomsByDate(date);

            Assert.IsInstanceOfType(result, typeof(IEnumerable<RoomModel>));
        }

        [TestMethod]
        public void FreeRoomsbyDate_ShouldReturnFreeRoomsNow()
        {
            var date = new DateTime(2021, 3, 15);
            BookingServiceMock.Setup(a => a.FreeRoomsByDate(date)).Returns(new List<RoomDTO>());

            var result = freeRoomsController.FreeRoomsByDate(date);

            BookingServiceMock.Verify(a => a.FreeRoomsByDate(date));
        }

        [TestMethod]
        public void FreeRoomsNowIsIEnumerableRoomModel()
        {
            BookingServiceMock.Setup(a => a.FreeRoomsByDate(DateTime.Now)).Returns(new List<RoomDTO>());

            var result = freeRoomsController.FreeRoomsNow();

            Assert.IsInstanceOfType(result, typeof(IEnumerable<RoomModel>));
        }


        [TestMethod]
        public void FreeRoomsByDateRangeIsIEnumerableRoomModel()
        {
            var date1 = new DateTime(2021, 3, 15);
            var date2 = new DateTime(2021, 3, 15);

            var date = new DateTime(2021, 3, 15);
            BookingServiceMock.Setup(a => a.FreeRoomsByDateRange(date1, date2)).Returns(new List<RoomDTO>());

            var result = freeRoomsController.FreeRoomsByDateRange(date1, date2);

            Assert.IsInstanceOfType(result, typeof(IEnumerable<RoomModel>));

        }

        [TestMethod]
        public void FreeRoomsByDateRange_ShouldReturnFreeRoomsByDateRange()
        {
            var date1 = new DateTime(2021, 3, 15);
            var date2 = new DateTime(2021, 3, 15);

            var date = new DateTime(2021, 3, 15);
            BookingServiceMock.Setup(a => a.FreeRoomsByDateRange(date1, date2)).Returns(new List<RoomDTO>());

            var result = freeRoomsController.FreeRoomsByDateRange(date1, date2);

            BookingServiceMock.Verify(a => a.FreeRoomsByDateRange(date1, date2));
        }
    }
}
