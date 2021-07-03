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
    public class BaseService : IBaseService
    {
        IWorkUnit Database;
        IMapper Mapper;
        public BaseService(IWorkUnit Database)
        {
            this.Database = Database;

            Mapper = new MapperConfiguration(
                   cfg =>
                   {
                       cfg.CreateMap<BookingDTO, Booking>().ReverseMap();
                       cfg.CreateMap<RoomDTO, Room>().ReverseMap();
                       cfg.CreateMap<GuestDTO, Guest>().ReverseMap();
                       cfg.CreateMap<CategoryDTO, Category>().ReverseMap();
                       cfg.CreateMap<PriceCategoryDTO, PriceCategory>().ReverseMap();
                   }).CreateMapper();

        }

        public IEnumerable<ProfitByMonthDTO> GetProfitByMonths()
        {
            DateTime HotelOpeningDate = new DateTime(2019, 2, 1);

            List<ProfitByMonthDTO> profitByMonths = new List<ProfitByMonthDTO>();

            for(DateTime counter = HotelOpeningDate; counter < DateTime.Now ; counter = counter.AddMonths(1))
            {
                profitByMonths.Add(GetProfitByOneMonth(counter));
            }

            return profitByMonths;
        }

        public ProfitByMonthDTO GetProfitByOneMonth(DateTime month)
        {
            ProfitByMonthDTO ProfitByMonth = new ProfitByMonthDTO();
            ProfitByMonth.Month = month;
            decimal profit = 0;
            int days;
            IEnumerable<PriceCategoryDTO> priceCategoriesForRoom;


            var bookings = Database.Bookings.GetAll();
            var priceCategories = Database.PriceCategories.GetAll();


            Interval MonthInterval = new Interval();
            MonthInterval.Start = month;
            MonthInterval.End = month.AddMonths(1);

            Interval BookingInterval = new Interval();
            Interval PriceCategoryInterval = new Interval();

            foreach (var booking in bookings)
            {    
                BookingInterval.Start = booking.EnterDate;
                BookingInterval.End = booking.LeaveDate; 
                                                          
                
                if(MonthInterval.Includes(BookingInterval))
                {
                    days = MonthInterval.DaysIncludes(BookingInterval);

                    priceCategoriesForRoom = Mapper.Map<IEnumerable<PriceCategory>, IEnumerable<PriceCategoryDTO>>(priceCategories.Where(p => p.CategoryId == booking.room.CategoryId));

                    foreach(var priceCategory in priceCategoriesForRoom)
                    {
                        PriceCategoryInterval.Start = priceCategory.StartDate;
                        PriceCategoryInterval.End = priceCategory.EndDate;

                        if(PriceCategoryInterval.Includes(BookingInterval)) 
                        {
                            profit += priceCategory.Price * BookingInterval.DaysIncludes(PriceCategoryInterval); 
                        }
                    }                    
                }
            }

            ProfitByMonth.Profit = profit;
            return ProfitByMonth;
        }

        public IEnumerable<RoomDTO> FreeRoomsByDate(DateTime date)
        {
            List<RoomDTO> BookedRoomsForDate = new List<RoomDTO>();
            var bookings = Mapper.Map<IEnumerable<Booking>, IEnumerable<BookingDTO>>(Database.Bookings.GetAll());
            var AllRooms = Mapper.Map<IEnumerable<Room>, IEnumerable<RoomDTO>>(Database.Rooms.GetAll());

            foreach(var booking in bookings)
            {
                if(booking.BookingDate <= date && booking.LeaveDate > date)
                {
                    BookedRoomsForDate.Add(AllRooms.FirstOrDefault(r=> r.Id == booking.room.Id));
                }
            }

            var NotBookedRooms = AllRooms.Except(BookedRoomsForDate);

            return NotBookedRooms;
        }

        public IEnumerable<RoomDTO> FreeRoomsByDateRange(DateTime firstDate, DateTime secondDate)
        {
            List<RoomDTO> BookedRoomsForDate = new List<RoomDTO>();
            var bookings = Mapper.Map<IEnumerable<Booking>, IEnumerable<BookingDTO>>(Database.Bookings.GetAll());
            var AllRooms = Mapper.Map<IEnumerable<Room>, IEnumerable<RoomDTO>>(Database.Rooms.GetAll());

            foreach (var booking in bookings)
            {
                if (booking.BookingDate <= firstDate && booking.LeaveDate > firstDate ||
                    booking.BookingDate <= firstDate && booking.LeaveDate > secondDate ||
                    booking.BookingDate >= firstDate && booking.LeaveDate < secondDate )
                {
                    BookedRoomsForDate.Add(AllRooms.FirstOrDefault(r => r.Id == booking.room.Id));
                }
            }

            var NotBookedRooms = AllRooms.Except(BookedRoomsForDate);

            return NotBookedRooms;
        }

        public void DoBooking(BookingDTO bookingDTO)
        {
            if (Database.Rooms.Get(bookingDTO.room.Id) == null ||
                Database.Guests.Get(bookingDTO.guest.Id) == null)
                throw new ArgumentException();

            if(!FreeRoomsByDateRange(bookingDTO.BookingDate,bookingDTO.LeaveDate).Any(freeRoom=>  bookingDTO.room.Id == freeRoom.Id ))
                throw new ArgumentException();

            Database.Bookings.Create(Mapper.Map<BookingDTO, Booking>(bookingDTO));
            Database.Save();

        }
    }

    class Interval
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool Includes(Interval t)
        {
            return t.Start >= Start && t.Start <= End ||
                t.End <= End && t.End >= Start ||
                t.Start <= Start && t.End >= End;
        }

        public int DaysIncludes(Interval t)
        {
            int DaysInMonth = (End - Start).Days;

            if (!Includes(t))
                return 0;

            uint d1 = (t.Start - Start).Days < 0 ? 0 : (uint)(t.Start - Start).Days;
            uint d2 = (End - t.End).Days < 0 ? 0 : (uint)(End - t.End).Days;    

            return DaysInMonth - (int)d1 - (int)d2; 
        }
    }
}
