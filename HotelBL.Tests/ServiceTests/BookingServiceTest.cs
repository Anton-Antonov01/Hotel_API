using AutoMapper;
using Hotel_BL.DTO;
using Hotel_BL.Services;
using Hotel_DAL.Entities;
using Hotel_DAL.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelBL.Tests.ServiceTests
{
    [TestClass]
    public class BookingServiceTest
    {
        private readonly BookingService bookingService;
        private readonly RoomService roomService;
        private readonly GuestService guestService;
        private readonly Mock<IWorkUnit> EFWorkUnitMock = new Mock<IWorkUnit>();
        private IMapper Mapper;
        private DataForTests dataForTests;

        public BookingServiceTest()
        {
            bookingService = new BookingService(EFWorkUnitMock.Object);
            roomService = new RoomService(EFWorkUnitMock.Object);
            guestService = new GuestService(EFWorkUnitMock.Object);

            Mapper = new MapperConfiguration(
                cfg => {
                    cfg.CreateMap<BookingDTO, Booking>().ReverseMap();
                    cfg.CreateMap<RoomDTO, Room>().ReverseMap();
                    cfg.CreateMap<GuestDTO, Guest>().ReverseMap();
                    cfg.CreateMap<CategoryDTO, Category>().ReverseMap();
                }
                ).CreateMapper();

            dataForTests = new DataForTests();
        }


        [TestMethod]
        public void GetByIdMethodIsBookingDTO()
        {
            //Arrange
            var bookingId = 1;
            EFWorkUnitMock.Setup(x => x.Bookings.Get(bookingId)).Returns(dataForTests.Bookings.SingleOrDefault(b => b.Id == bookingId));

            //Act
            var result = bookingService.Get(bookingId);

            Assert.IsInstanceOfType(result, typeof(BookingDTO));
        }

        [TestMethod]
        public void GetById_ShouldReturnBooking_WhenBookingExists()
        {
            //Arrange
            var bookingId = 1;
            EFWorkUnitMock.Setup(x => x.Bookings.Get(bookingId)).Returns(dataForTests.Bookings.SingleOrDefault(b => b.Id == bookingId));

            //Act
            var result = bookingService.Get(bookingId);
            var expected = Mapper.Map<Booking, BookingDTO>(dataForTests.Bookings.SingleOrDefault(b => b.Id == bookingId));

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
         "no Booking with this id.")]
        public void GetById_ShouldThrowNullReferenceException_WhenBookingWithIdNotExsists() 
        {
            //Arrange
            int BookingId = 5;
            EFWorkUnitMock.Setup(x => x.Bookings.Get(BookingId)).Returns(dataForTests.Bookings.SingleOrDefault(r => r.Id == BookingId));

            //Act
            var guestResult = bookingService.Get(BookingId); 
        }


        [TestMethod]
        public void GetAllMethodIsIEnumerableBookingDTO()
        {
            //Arrange
            EFWorkUnitMock.Setup(x => x.Bookings.GetAll()).Returns(dataForTests.Bookings);

            //Act
            var result = bookingService.GetAllBookings();

            Assert.IsInstanceOfType(result, typeof(IEnumerable<BookingDTO>));
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllBookings()
        {
            //Arrange
            EFWorkUnitMock.Setup(x => x.Bookings.GetAll()).Returns(dataForTests.Bookings);

            //Act
            var bookingResult = bookingService.GetAllBookings();
            IEnumerable<BookingDTO> expected = Mapper.Map<IEnumerable<Booking>, IEnumerable<BookingDTO>>(EFWorkUnitMock.Object.Bookings.GetAll());

            //Assert
            CollectionAssert.AreEqual(expected.ToList(), bookingResult.ToList());
        }



        [DataTestMethod]
        [DataRow("2021-03-11", "2021-03-13")]
        [DataRow("2021-03-10", "2021-03-13")]
        public void AddBooking_ShouldAddBooking(string start, string end)
        {
            //Arrange
            var RoomId = 1;
            var GuestId = 3;

            BookingDTO bookingDTO = new BookingDTO {
                Id = 1,
                Set = false,
                BookingDate = Convert.ToDateTime(start),
                EnterDate = Convert.ToDateTime(start),
                LeaveDate = Convert.ToDateTime(end),
                room = Mapper.Map<Room, RoomDTO>(dataForTests.Rooms.SingleOrDefault(r => r.Id == RoomId)),
                guest = Mapper.Map<Guest, GuestDTO>(dataForTests.Guests.SingleOrDefault(r => r.Id == GuestId)),
            };


            EFWorkUnitMock.Setup(x => x.Bookings.Create(Mapper.Map<BookingDTO, Booking>(bookingDTO)));
            EFWorkUnitMock.Setup(x => x.Rooms.GetAll()).Returns(dataForTests.Rooms);
            EFWorkUnitMock.Setup(x => x.Guests.GetAll()).Returns(dataForTests.Guests);
            EFWorkUnitMock.Setup(x => x.Rooms.Get(RoomId)).Returns(dataForTests.Rooms.SingleOrDefault(c => c.Id == RoomId));
            EFWorkUnitMock.Setup(x => x.Guests.Get(GuestId)).Returns(dataForTests.Guests.SingleOrDefault(g => g.Id == GuestId));


            //Act
            bookingService.AddBooking(bookingDTO);

            //Assert
            EFWorkUnitMock.Verify(x => x.Bookings.Create(Mapper.Map<BookingDTO, Booking>(bookingDTO)));
        }



        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
         "Room with RoomId not Exsist")]
        public void AddBooking_ShouldThrowNullReferenceException_WhenRoomNotExsists()
        {
            var RoomId = 111;
            var GuestId = 4;

            BookingDTO bookingDTO = new BookingDTO
            {
                Id = 1,
                Set = false,
                BookingDate = new DateTime(2021, 3, 5),
                EnterDate = new DateTime(2021, 3, 6),
                LeaveDate = new DateTime(2021, 3, 10),
                room = Mapper.Map<Room, RoomDTO>(dataForTests.Rooms.SingleOrDefault(r => r.Id == RoomId)),
                guest = Mapper.Map<Guest, GuestDTO>(dataForTests.Guests.SingleOrDefault(r => r.Id == GuestId)),
            };


            EFWorkUnitMock.Setup(x => x.Bookings.Create(Mapper.Map<BookingDTO, Booking>(bookingDTO)));
            EFWorkUnitMock.Setup(x => x.Rooms.GetAll()).Returns(dataForTests.Rooms);
            EFWorkUnitMock.Setup(x => x.Guests.GetAll()).Returns(dataForTests.Guests);
            EFWorkUnitMock.Setup(x => x.Rooms.Get(RoomId)).Returns(dataForTests.Rooms.SingleOrDefault(c => c.Id == RoomId));
            EFWorkUnitMock.Setup(x => x.Guests.Get(GuestId)).Returns(dataForTests.Guests.SingleOrDefault(g => g.Id == GuestId));


            //Act
            bookingService.AddBooking(bookingDTO);
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
         "Guest with Guest not Exsist")]
        public void AddBooking_ShouldThrowNullReferenceException_WhenGuestNotExsists()
        {
            var RoomId = 4;
            var GuestId = 1111;

            BookingDTO bookingDTO = new BookingDTO
            {
                Id = 1,
                Set = false,
                BookingDate = new DateTime(2021, 3, 5),
                EnterDate = new DateTime(2021, 3, 6),
                LeaveDate = new DateTime(2021, 3, 10),
                room = Mapper.Map<Room, RoomDTO>(dataForTests.Rooms.SingleOrDefault(r => r.Id == RoomId)),
                guest = Mapper.Map<Guest, GuestDTO>(dataForTests.Guests.SingleOrDefault(r => r.Id == GuestId)),
            };

            EFWorkUnitMock.Setup(x => x.Bookings.Create(Mapper.Map<BookingDTO, Booking>(bookingDTO)));
            EFWorkUnitMock.Setup(x => x.Rooms.GetAll()).Returns(dataForTests.Rooms);
            EFWorkUnitMock.Setup(x => x.Guests.GetAll()).Returns(dataForTests.Guests);
            EFWorkUnitMock.Setup(x => x.Rooms.Get(RoomId)).Returns(dataForTests.Rooms.SingleOrDefault(c => c.Id == RoomId));
            EFWorkUnitMock.Setup(x => x.Guests.Get(GuestId)).Returns(dataForTests.Guests.SingleOrDefault(g => g.Id == GuestId));

            //Act
            bookingService.AddBooking(bookingDTO);
        }




        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
          "The room has already been booked for these dates")]
        [DataTestMethod]
        [DataRow("2021-03-7", "2021-03-9")]
        [DataRow("2021-03-5", "2021-03-10")]
        [DataRow("2021-03-1", "2021-03-9")]
        [DataRow("2021-03-5", "2021-03-8")]
        [DataRow("2021-03-7", "2021-03-10")]
        [DataRow("2021-03-7", "2021-03-13")]
        [DataRow("2021-03-1", "2021-03-13")]
        [DataRow("2021-03-5", "2021-03-13")]
        public void AddBooking_ShouldThrowNullReferenceException_WhereBookingsDatesForOneRoomOverlap(string start, string end)
        {
            var RoomId = 1;
            var GuestId = 1;

            BookingDTO bookingDTO = new BookingDTO
            {
                Id = 1,
                Set = false,
                BookingDate = Convert.ToDateTime(start),
                EnterDate = Convert.ToDateTime(start),
                LeaveDate = Convert.ToDateTime(end),
                room = Mapper.Map<Room, RoomDTO>(dataForTests.Rooms.SingleOrDefault(r => r.Id == RoomId)),
                guest = Mapper.Map<Guest, GuestDTO>(dataForTests.Guests.SingleOrDefault(r => r.Id == GuestId)),
            };

            EFWorkUnitMock.Setup(x => x.Bookings.Create(Mapper.Map<BookingDTO, Booking>(bookingDTO)));
            EFWorkUnitMock.Setup(x => x.Rooms.GetAll()).Returns(dataForTests.Rooms);
            EFWorkUnitMock.Setup(x => x.Guests.GetAll()).Returns(dataForTests.Guests);
            EFWorkUnitMock.Setup(x => x.Bookings.GetAll()).Returns(dataForTests.Bookings);
            EFWorkUnitMock.Setup(x => x.Rooms.Get(RoomId)).Returns(dataForTests.Rooms.SingleOrDefault(c => c.Id == RoomId));
            EFWorkUnitMock.Setup(x => x.Guests.Get(GuestId)).Returns(dataForTests.Guests.SingleOrDefault(g => g.Id == GuestId));


            //Act
            bookingService.AddBooking(bookingDTO);
        }



        [TestMethod]
        public void DeleteBooking_ShouldDeleteBooking_WhenBookingExsists()
        {
            //Arrange
            int BookingId = 1;
            EFWorkUnitMock.Setup(x => x.Bookings.Delete(BookingId));
            EFWorkUnitMock.Setup(x => x.Bookings.Get(BookingId)).Returns(dataForTests.Bookings.SingleOrDefault(b => b.Id == BookingId));

            //Act
            bookingService.DeleteBooking(BookingId);

            //Assert
            EFWorkUnitMock.Verify(x => x.Bookings.Delete(BookingId));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
         "no Booking with this id.")]
        public void DeleteBooking_ShouldThrowNullReferenceException_WhenBookingNotExsists()
        {
            //Arrange
            int BookingId = 111;
            EFWorkUnitMock.Setup(x => x.Bookings.Delete(BookingId));
            EFWorkUnitMock.Setup(x => x.Bookings.Get(BookingId)).Returns(dataForTests.Bookings.SingleOrDefault(b => b.Id == BookingId));

            //Act
            bookingService.DeleteBooking(BookingId);

            //Assert
            EFWorkUnitMock.Verify(x => x.Bookings.Delete(BookingId));
        }

        [DataTestMethod]
        [DataRow("2021-03-5", "2021-03-10")]
        [DataRow("2021-03-10", "2021-03-15")]
        public void UpdateBooking_ShouldUpdateBooking_WhenBookingExsists(string start, string end)
        {
            var BookingId = 1;
            var RoomId = 4;
            var GuestId = 4;

            BookingDTO bookingDTO = new BookingDTO
            {
                Id = BookingId,
                Set = false,
                BookingDate = Convert.ToDateTime(start),
                EnterDate = Convert.ToDateTime(start),
                LeaveDate = Convert.ToDateTime(end),
                room = Mapper.Map<Room, RoomDTO>(dataForTests.Rooms.SingleOrDefault(r => r.Id == RoomId)),
                guest = Mapper.Map<Guest, GuestDTO>(dataForTests.Guests.SingleOrDefault(r => r.Id == GuestId)),
            };


            EFWorkUnitMock.Setup(x => x.Bookings.Update(Mapper.Map<BookingDTO, Booking>(bookingDTO)));
            EFWorkUnitMock.Setup(x => x.Rooms.GetAll()).Returns(dataForTests.Rooms);
            EFWorkUnitMock.Setup(x => x.Guests.GetAll()).Returns(dataForTests.Guests);
            EFWorkUnitMock.Setup(x => x.Bookings.Get(BookingId)).Returns(dataForTests.Bookings.SingleOrDefault(b => b.Id == BookingId));
            EFWorkUnitMock.Setup(x => x.Rooms.Get(RoomId)).Returns(dataForTests.Rooms.SingleOrDefault(c => c.Id == RoomId));
            EFWorkUnitMock.Setup(x => x.Guests.Get(GuestId)).Returns(dataForTests.Guests.SingleOrDefault(g => g.Id == GuestId));


            //Act
            bookingService.UpdateBooking(bookingDTO);

            //Assert
            EFWorkUnitMock.Verify(x => x.Bookings.Update(Mapper.Map<BookingDTO, Booking>(bookingDTO)));
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
         "no room with this id.")]
        public void UpdateBooking_ShouldThrowNullReferenceException_WhenRoomNotExsists()
        {
            var BookingId = 1;
            var RoomId = 111;
            var GuestId = 4;

            BookingDTO bookingDTO = new BookingDTO
            {
                Id = BookingId,
                Set = false,
                BookingDate = new DateTime(2021, 3, 5),
                EnterDate = new DateTime(2021, 3, 6),
                LeaveDate = new DateTime(2021, 3, 10),
                room = Mapper.Map<Room, RoomDTO>(dataForTests.Rooms.SingleOrDefault(r => r.Id == RoomId)),
                guest = Mapper.Map<Guest, GuestDTO>(dataForTests.Guests.SingleOrDefault(r => r.Id == GuestId)),
            };


            EFWorkUnitMock.Setup(x => x.Bookings.Update(Mapper.Map<BookingDTO, Booking>(bookingDTO)));
            EFWorkUnitMock.Setup(x => x.Rooms.GetAll()).Returns(dataForTests.Rooms);
            EFWorkUnitMock.Setup(x => x.Guests.GetAll()).Returns(dataForTests.Guests);
            EFWorkUnitMock.Setup(x => x.Bookings.Get(BookingId)).Returns(dataForTests.Bookings.SingleOrDefault(b => b.Id == BookingId));
            EFWorkUnitMock.Setup(x => x.Rooms.Get(RoomId)).Returns(dataForTests.Rooms.SingleOrDefault(c => c.Id == RoomId));
            EFWorkUnitMock.Setup(x => x.Guests.Get(GuestId)).Returns(dataForTests.Guests.SingleOrDefault(g => g.Id == GuestId));


            //Act
            bookingService.UpdateBooking(bookingDTO);
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
           "no guest with this id.")]
        public void UpdateBooking_ShouldThrowNullReferenceException_WhenGuestNotExsists()
        {
            var BookingId = 1;
            var RoomId = 4;
            var GuestId = 1111;

            BookingDTO bookingDTO = new BookingDTO
            {
                Id = BookingId,
                Set = false,
                BookingDate = new DateTime(2021, 3, 5),
                EnterDate = new DateTime(2021, 3, 6),
                LeaveDate = new DateTime(2021, 3, 10),
                room = Mapper.Map<Room, RoomDTO>(dataForTests.Rooms.SingleOrDefault(r => r.Id == RoomId)),
                guest = Mapper.Map<Guest, GuestDTO>(dataForTests.Guests.SingleOrDefault(r => r.Id == GuestId)),
            };


            EFWorkUnitMock.Setup(x => x.Bookings.Update(Mapper.Map<BookingDTO, Booking>(bookingDTO)));
            EFWorkUnitMock.Setup(x => x.Rooms.GetAll()).Returns(dataForTests.Rooms);
            EFWorkUnitMock.Setup(x => x.Guests.GetAll()).Returns(dataForTests.Guests);
            EFWorkUnitMock.Setup(x => x.Bookings.Get(BookingId)).Returns(dataForTests.Bookings.SingleOrDefault(b => b.Id == BookingId));
            EFWorkUnitMock.Setup(x => x.Rooms.Get(RoomId)).Returns(dataForTests.Rooms.SingleOrDefault(c => c.Id == RoomId));
            EFWorkUnitMock.Setup(x => x.Guests.Get(GuestId)).Returns(dataForTests.Guests.SingleOrDefault(g => g.Id == GuestId));


            //Act
            bookingService.UpdateBooking(bookingDTO);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
          "The room has already been booked for these dates")]
        [DataTestMethod]
        [DataRow("2021-03-7", "2021-03-9")]
        [DataRow("2021-03-5", "2021-03-10")]
        [DataRow("2021-03-1", "2021-03-9")]
        [DataRow("2021-03-5", "2021-03-8")]
        [DataRow("2021-03-7", "2021-03-10")]
        [DataRow("2021-03-7", "2021-03-13")]
        [DataRow("2021-03-1", "2021-03-13")]
        [DataRow("2021-03-5", "2021-03-13")]
        public void UpdateBooking_ShouldThrowNullReferenceException_WhenRoomAlreadyBooking_NewBookingDatesIntoOld(string start, string end)
        {
            var BookingId = 1;
            var RoomId = 1;
            var GuestId = 1;

            BookingDTO bookingDTO = new BookingDTO
            {
                Id = BookingId,
                Set = false,
                BookingDate = Convert.ToDateTime(start),
                EnterDate = Convert.ToDateTime(start),
                LeaveDate = Convert.ToDateTime(end),
                room = Mapper.Map<Room, RoomDTO>(dataForTests.Rooms.SingleOrDefault(r => r.Id == RoomId)),
                guest = Mapper.Map<Guest, GuestDTO>(dataForTests.Guests.SingleOrDefault(r => r.Id == GuestId)),
            };

            EFWorkUnitMock.Setup(x => x.Bookings.Update(Mapper.Map<BookingDTO, Booking>(bookingDTO)));
            EFWorkUnitMock.Setup(x => x.Rooms.GetAll()).Returns(dataForTests.Rooms);
            EFWorkUnitMock.Setup(x => x.Guests.GetAll()).Returns(dataForTests.Guests);
            EFWorkUnitMock.Setup(x => x.Bookings.GetAll()).Returns(dataForTests.Bookings);
            EFWorkUnitMock.Setup(x => x.Bookings.Get(BookingId)).Returns(dataForTests.Bookings.SingleOrDefault(b => b.Id == BookingId));
            EFWorkUnitMock.Setup(x => x.Rooms.Get(RoomId)).Returns(dataForTests.Rooms.SingleOrDefault(c => c.Id == RoomId));
            EFWorkUnitMock.Setup(x => x.Guests.Get(GuestId)).Returns(dataForTests.Guests.SingleOrDefault(g => g.Id == GuestId));

            //Act
            bookingService.UpdateBooking(bookingDTO);
        }
    }
}
