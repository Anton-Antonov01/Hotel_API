using Hotel_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_DAL.EF
{
    public class HotelInitializer : DropCreateDatabaseAlways<HotelContext>
    {
        private void CategoryInitializer(HotelContext context)
        {
            var categoryList = new List<Category>()
            {
                new Category()
                {
                    Name = "Standard",
                    Bed = 1
                   
                },
                new Category()
                {
                    Name = "Standard",
                    Bed = 2
                },
                new Category()
                {
                    Name = "Luxe",
                    Bed = 1

                },
                new Category()
                {
                    Name = "Luxe",
                    Bed = 2
                },
            };

            foreach (var category in categoryList)
            {
                context.Categories.Add(category);
            }

            context.SaveChanges();
        }

        private void RoomInitializer(HotelContext context)
        {
            var roomList = new List<Room>()
            {
                new Room()
                {
                    Name = "101",
                    CategoryId = 1,
                    Active = true
                },

                new Room()
                {
                    Name = "102",
                    CategoryId = 2,
                    Active = true
                },

                new Room()
                {
                    Name = "201",
                    CategoryId = 3,
                    Active = true
                },

                new Room()
                {
                    Name = "202",
                    CategoryId = 4,
                    Active = true
                }
            };


            foreach (var room in roomList)
            {
                context.Rooms.Add(room);
            }

            context.SaveChanges();
        }

        private void GuestInitializer(HotelContext context)
        {
            var guestList = new List<Guest>()
            {
                new Guest()
                {
                    Name = "Anton",
                    Surname = "Antonov",
                    Address = "Kharkiv, Victory Avenue, 33a, 65",
                    Phone = "0684545445"

                },
                new Guest()
                {
                    Name = "Oleg",
                    Surname = "Petrov",
                    Address = "Kharkiv, Sumskaya street, 133a, 25",
                    Phone = "0684123345"
                },

                new Guest()
                {
                    Name = "Vadim",
                    Surname = "Arshava",
                    Address = "Kharkiv, Hrushevsky street, 21, 15",
                    Phone = "0684777445"

                },
                new Guest()
                {
                    Name = "Sergey",
                    Surname = "Lavrinenko",
                    Address = "Kharkiv, Shevchenko street, 77, 7",
                    Phone = "0684666345"
                }
            };

            foreach (var guest in guestList)
            {
                context.Guests.Add(guest);
            }

            context.SaveChanges();
        }

        private void BookingInitializer(HotelContext context)
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
                },

                new Booking()
                {
                    Id = 1,
                    GuestId = 2,
                    RoomId = 1,
                    Set = true,
                    BookingDate = new DateTime(2021, 3, 15),
                    EnterDate = new DateTime(2021, 3, 16),
                    LeaveDate = new DateTime(2021, 3, 20),
                },

                new Booking()
                {
                    Id = 1,
                    GuestId = 3,
                    RoomId = 2,
                    Set = true,
                    BookingDate = new DateTime(2021, 4, 2),
                    EnterDate = new DateTime(2021, 4, 10),
                    LeaveDate = new DateTime(2021, 4, 20),
                },

                new Booking()
                {
                    Id = 1,
                    GuestId = 4,
                    RoomId = 3,
                    Set = true,
                    BookingDate = new DateTime(2021, 4, 15),
                    EnterDate = new DateTime(2021, 4, 16),
                    LeaveDate = new DateTime(2021, 5, 20),
                },

            };


            foreach (var booking in BookingList)
            {
                context.Bookings.Add(booking);
            }

            context.SaveChanges();
        }

        private void PriceCategoryInitializer(HotelContext context)
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
                },

                new PriceCategory()
                {
                    Id = 2,
                    CategoryId = 2,
                    Price = 200,
                    StartDate = new DateTime(2017, 3, 15),
                    EndDate = new DateTime(2022, 3, 15),
                },

                new PriceCategory()
                {
                    Id = 3,
                    CategoryId = 3,
                    Price = 300,
                    StartDate = new DateTime(2017, 3, 15),
                    EndDate = new DateTime(2021, 4, 20),
                },

                new PriceCategory()
                {
                    Id = 3,
                    CategoryId = 3,
                    Price = 1000,
                    StartDate = new DateTime(2021, 4, 20),
                    EndDate = new DateTime(2022, 3, 15),
                },

                new PriceCategory()
                {
                    Id = 4,
                    CategoryId = 4,
                    Price = 400,
                    StartDate = new DateTime(2017, 3, 15),
                    EndDate = new DateTime(2022, 3, 15),
                },
            };


            foreach (var priceCategory in priceCategories)
            {
                context.PriceCategories.Add(priceCategory);
            }

            context.SaveChanges();
        }

        protected override void Seed(HotelContext context)
        {
            CategoryInitializer(context);
            RoomInitializer(context);
            GuestInitializer(context);
            BookingInitializer(context);
            PriceCategoryInitializer(context);
        }
    }

}
