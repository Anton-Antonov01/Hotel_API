using Hotel_DAL.EF;
using Hotel_DAL.Entities;
using Hotel_DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_DAL.Repositories
{
    public class BookingRepository : IRepository<Booking>
    {
        HotelContext db;

        public BookingRepository(HotelContext db)
        {
            this.db = db;
        }

        public Booking Get(int id)
        {
            return db.Bookings.Find(id);
        }

        public IEnumerable<Booking> GetAll()
        {
            return db.Bookings;
        }

        public void Create(Booking booking)
        {
            booking.guest = db.Guests.Find(booking.guest.Id);
            booking.room = db.Rooms.Find(booking.room.Id);

            db.Bookings.Add(booking);
        }

        public void Delete(int id)
        {
            db.Bookings.Remove(Get(id));
        }

        public void Update(Booking booking)
        {
            var toUpdate = db.Bookings.FirstOrDefault(m => m.Id == booking.Id);
            if (toUpdate != null)
            {
                toUpdate.guest = db.Guests.Find(booking.guest.Id) ?? toUpdate.guest;
                toUpdate.room = db.Rooms.Find(booking.room.Id) ?? toUpdate.room;

                toUpdate.Id = booking.Id;
                toUpdate.BookingDate = booking.BookingDate;
                toUpdate.EnterDate = booking.EnterDate;
                toUpdate.LeaveDate = booking.LeaveDate;
                toUpdate.Set = booking.Set;
                toUpdate.RoomId = booking.RoomId;
                toUpdate.GuestId = booking.GuestId;
            }
        }
    }
}
