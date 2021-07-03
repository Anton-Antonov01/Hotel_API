using Hotel_DAL.Entities;
using System;
using System.Data.Entity;
using System.Linq;

namespace Hotel_DAL.EF
{
    public class HotelContext : DbContext
    {
        public HotelContext(string connectionString)
            : base(connectionString)
        {
            Database.SetInitializer<HotelContext>(new HotelInitializer());
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet <Booking> Bookings { get; set; }
        public DbSet<PriceCategory> PriceCategories { get; set; }

    }


}