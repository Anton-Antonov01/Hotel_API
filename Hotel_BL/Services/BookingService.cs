using AutoMapper;
using Hotel_BL.DTO;
using Hotel_BL.Interfaces;
using Hotel_DAL.Entities;
using Hotel_DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_BL.Services
{
    public class BookingService : IBookingService
    {
        IWorkUnit Database;
        IMapper Mapper;

        public BookingService(IWorkUnit database)
        {
            Database = database;

            Mapper = new MapperConfiguration(
                cfg => {
                    cfg.CreateMap<BookingDTO, Booking>().ReverseMap();
                    cfg.CreateMap<RoomDTO, Room>().ReverseMap();
                    cfg.CreateMap<GuestDTO, Guest>().ReverseMap();
                    cfg.CreateMap<CategoryDTO, Category>().ReverseMap();
                })
                .CreateMapper();
        }

        public BookingDTO Get(int id)
        {
            var booking = (Database.Bookings.Get(id));
            if (booking == null)
                throw new ArgumentException();

            return Mapper.Map<Booking,BookingDTO>(booking);
        }

        public IEnumerable<BookingDTO> GetAllBookings()
        {
            var bookings = Database.Bookings.GetAll();

            return Mapper.Map<IEnumerable<Booking>, IEnumerable<BookingDTO>>(bookings);
        }

        public void AddBooking(BookingDTO bookingDTO)
        {
            if(Database.Rooms.Get(bookingDTO.room.Id) == null ||
                Database.Guests.Get(bookingDTO.guest.Id) == null)
                throw new ArgumentException();


            Database.Bookings.Create(Mapper.Map<BookingDTO, Booking>(bookingDTO));
            Database.Save();
        }

        public void DeleteBooking(int id)
        {
            if (Database.Bookings.Get(id) == null)
                throw new ArgumentException();

            Database.Bookings.Delete(id);
            Database.Save();
        }

        public void UpdateBooking(BookingDTO bookingDTO)
        {
            if (Database.Bookings.Get(bookingDTO.Id) == null ||
                Database.Rooms.Get(bookingDTO.room.Id) == null ||
                Database.Guests.Get(bookingDTO.guest.Id) == null)
                throw new ArgumentException();

            Database.Bookings.Update(Mapper.Map<BookingDTO, Booking>(bookingDTO));
            Database.Save();
        }
    }
}
