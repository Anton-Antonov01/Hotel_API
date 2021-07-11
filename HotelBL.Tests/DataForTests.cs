using Hotel_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBL.Tests
{
    class DataForTests
    {
        public List<Guest> Guests { get; set; }
        public List<Category> Categories { get; set; }
        public List<Room> Rooms { get; set; }
        public List<Booking> Bookings { get; set; }
        public List<PriceCategory> PriceCategories { get; set; }

        public DataForTests()
        {
            Guests = GuestInitializer();
            Categories = CategoryInitializer();
            Rooms = RoomInitializer();
            Bookings = BookingInitializer();
            PriceCategories = PriceCategoryInitializer();
        }

        private List<Category> CategoryInitializer()
        {
            var categoryList = new List<Category>()
            {
                new Category()
                {
                    Id = 1,
                    Name = "Standard",
                    Bed = 1

                },
                new Category()
                {
                    Id = 2,
                    Name = "Standard",
                    Bed = 2
                },
                new Category()
                {
                    Id = 3,
                    Name = "Luxe",
                    Bed = 1

                },
                new Category()
                {
                    Id = 4,
                    Name = "Luxe",
                    Bed = 2
                },
            };

            return categoryList;
        }

        private List<Room> RoomInitializer()
        {
            var roomList = new List<Room>()
            {
                new Room()
                {
                    Id = 1,
                    Name = "101",
                    CategoryId = 1,
                    Active = true,
                    RoomCategory = Categories.SingleOrDefault(c => c.Id == 1)
                },

                new Room()
                {
                    Id = 2,
                    Name = "102",
                    CategoryId = 2,
                    Active = true,
                    RoomCategory = Categories.SingleOrDefault(c => c.Id == 2)
                },

                new Room()
                {
                    Id = 3,
                    Name = "201",
                    CategoryId = 3,
                    Active = true,
                    RoomCategory = Categories.SingleOrDefault(c => c.Id == 3)
                },

                new Room()
                {
                    Id = 4,
                    Name = "202",
                    CategoryId = 4,
                    Active = true,
                    RoomCategory = Categories.SingleOrDefault(c => c.Id == 4)
                }
            };

            return roomList;
        }

        private List<Guest> GuestInitializer()
        {
            var guestList = new List<Guest>()
            {
                new Guest()
                {
                    Id = 1,
                    Name = "Anton",
                    Surname = "Antonov",
                    Address = "Kharkiv, Victory Avenue, 33a, 65",
                    Phone = "1111"
                },
                new Guest()
                {
                    Id = 2,
                    Name = "Oleg",
                    Surname = "Petrov",
                    Address = "Kharkiv, Sumskaya street, 133a, 25",
                    Phone = "2222"
                },

                new Guest()
                {
                    Id = 3,
                    Name = "Vadim",
                    Surname = "Arshava",
                    Address = "Kharkiv, Hrushevsky street, 21, 15",
                    Phone = "3333"

                },
                new Guest()
                {
                    Id = 4,
                    Name = "Sergey",
                    Surname = "Lavrinenko",
                    Address = "Kharkiv, Shevchenko street, 77, 7",
                    Phone = "4444"
                }
            };


            return guestList;
        }

        private List<Booking> BookingInitializer()
        {
            var BookingList = new List<Booking>()
            {
                new Booking()
                {
                    Id = 1,
                    GuestId = 1,
                    RoomId = 1,
                    Set = false,
                    BookingDate = new DateTime(2021, 3, 5),
                    EnterDate = new DateTime(2021, 3, 6),
                    LeaveDate = new DateTime(2021, 3, 10),
                    guest = Guests.SingleOrDefault(r=> r.Id == 1),
                    room = Rooms.SingleOrDefault(r=> r.Id == 1),

                },

                new Booking()
                {
                    Id = 2,
                    GuestId = 2,
                    RoomId = 1,
                    Set = true,
                    BookingDate = new DateTime(2021, 3, 15),
                    EnterDate = new DateTime(2021, 3, 16),
                    LeaveDate = new DateTime(2021, 3, 20),
                    guest = Guests.SingleOrDefault(r=> r.Id == 2),
                    room = Rooms.SingleOrDefault(r=> r.Id == 1),
                },

                new Booking()
                {
                    Id = 3,
                    GuestId = 3,
                    RoomId = 2,
                    Set = true,
                    BookingDate = new DateTime(2021, 4, 2),
                    EnterDate = new DateTime(2021, 4, 10),
                    LeaveDate = new DateTime(2021, 4, 20),
                    guest = Guests.SingleOrDefault(r=> r.Id == 3),
                    room = Rooms.SingleOrDefault(r=> r.Id == 2),
                },

                new Booking()
                {
                    Id = 4,
                    GuestId = 4,
                    RoomId = 3,
                    Set = true,
                    BookingDate = new DateTime(2021, 4, 15),
                    EnterDate = new DateTime(2021, 4, 16),
                    LeaveDate = new DateTime(2021, 5, 20),
                    guest = Guests.SingleOrDefault(r=> r.Id == 4),
                    room = Rooms.SingleOrDefault(r=> r.Id == 3),
                },

            };

            return BookingList;
        }

        private List<PriceCategory> PriceCategoryInitializer()
        {
            var priceCategories = new List<PriceCategory>()
            {
                new PriceCategory()
                {
                    Id = 1,
                    CategoryId = 1,
                    Price = 100,
                    StartDate = new DateTime(2017, 3, 15),
                    EndDate = new DateTime(2022, 3, 15),
                    Category = Categories.SingleOrDefault(c => c.Id== 1)
                },

                new PriceCategory()
                {
                    Id = 2,
                    CategoryId = 2,
                    Price = 200,
                    StartDate = new DateTime(2017, 3, 15),
                    EndDate = new DateTime(2022, 3, 15),
                    Category = Categories.SingleOrDefault(c => c.Id== 2)
                },

                new PriceCategory()
                {
                    Id = 3,
                    CategoryId = 3,
                    Price = 300,
                    StartDate = new DateTime(2017, 3, 15),
                    EndDate = new DateTime(2021, 4, 20),
                    Category = Categories.SingleOrDefault(c => c.Id== 3)
                },

                new PriceCategory()
                {
                    Id = 4,
                    CategoryId = 3,
                    Price = 1000,
                    StartDate = new DateTime(2021, 4, 20),
                    EndDate = new DateTime(2022, 3, 15),
                    Category = Categories.SingleOrDefault(c => c.Id== 3)
                },

                new PriceCategory()
                {
                    Id = 5,
                    CategoryId = 4,
                    Price = 400,
                    StartDate = new DateTime(2017, 3, 15),
                    EndDate = new DateTime(2022, 3, 15),
                    Category = Categories.SingleOrDefault(c => c.Id == 4)
                },
            };

            return priceCategories;
        }
    }
}
