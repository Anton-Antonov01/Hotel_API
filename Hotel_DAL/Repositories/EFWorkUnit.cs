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
    public class EFWorkUnit : IWorkUnit
    {
        private HotelContext db;
        private RoomRepository roomRepository;
        private CategoryRepository categoryRepository;
        private GuestRepository guestRepository;
        private BookingRepository bookingRepository;
        private PriceCategoryRepository priceCategoryRepository;


        public EFWorkUnit(string connectionString)
        {
            db = new HotelContext(connectionString);
        }

        public IRepository<Room> Rooms
        {
            get
            {
                if (roomRepository == null)
                {
                    roomRepository = new RoomRepository(db);
                }
                return roomRepository;
            }
        }

        public IRepository<Category> Categories
        {
            get
            {
                if (categoryRepository == null)
                {
                    categoryRepository = new CategoryRepository(db);
                }
                return categoryRepository;
            }
        }

        public IRepository<Guest> Guests
        {
            get
            {
                if (guestRepository == null)
                {
                    guestRepository = new GuestRepository(db);
                }
                return guestRepository;
            }
        }

        public IRepository<Booking> Bookings
        {
            get
            {
                if (bookingRepository == null)
                {
                    bookingRepository = new BookingRepository(db);
                }
                return bookingRepository;
            }
        }

        public IRepository<PriceCategory> PriceCategories
        {
            get
            {
                if (priceCategoryRepository == null)
                {
                    priceCategoryRepository = new PriceCategoryRepository(db);
                }
                return priceCategoryRepository;
            }
        }
        public void Save()
        {
            db.SaveChanges();
        }
    }
}
