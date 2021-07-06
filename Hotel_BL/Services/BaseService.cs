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
using Hotel_BL.HelpClasses;


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
            DateTime HotelOpeningDate = new DateTime(2021, 2, 1);

            List<ProfitByMonthDTO> profitByMonths = new List<ProfitByMonthDTO>();

            for(DateTime monthCounter = HotelOpeningDate; monthCounter < DateTime.Now ; monthCounter = monthCounter.AddMonths(1))
            {
                profitByMonths.Add(GetProfitByOneMonth(monthCounter));
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


            Interval MonthInterval = new Interval(month, month.AddMonths(1));


            Interval BookingInterval = new Interval();
            Interval PriceCategoryInterval = new Interval();

            foreach (var booking in bookings)
            {    
                BookingInterval.Start = booking.EnterDate.Date;
                BookingInterval.End = booking.LeaveDate.Date; 
                                                          
                
                if(MonthInterval.IsInclude(BookingInterval))
                {
                    days = MonthInterval.DaysIncludes(BookingInterval);//колличество дней в месяце, в которых была эта бронь

                    priceCategoriesForRoom = Mapper.Map<IEnumerable<PriceCategory>, IEnumerable<PriceCategoryDTO>>(priceCategories.Where(p => p.CategoryId == booking.room.CategoryId));

                    foreach(var priceCategory in priceCategoriesForRoom)
                    {
                        PriceCategoryInterval.Start = priceCategory.StartDate;
                        PriceCategoryInterval.End = priceCategory.EndDate;

                        if (PriceCategoryInterval.IsInclude(BookingInterval)) 
                        {
                            profit += priceCategory.Price * (MonthInterval.DaysIncludes(BookingInterval.IncludeDate(PriceCategoryInterval))); 
                        }
                    }                    
                }
            }

            ProfitByMonth.Profit = profit;
            return ProfitByMonth;
        }




    }
}
