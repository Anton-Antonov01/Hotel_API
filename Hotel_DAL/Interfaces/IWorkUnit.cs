using Hotel_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_DAL.Interfaces
{
    public interface IWorkUnit
    {
        IRepository<Room> Rooms { get; }
        IRepository<Category> Categories { get; }
        IRepository<Guest> Guests { get; }
        IRepository<Booking> Bookings { get; }
        IRepository<PriceCategory> PriceCategories { get; }
        void Save();
    }
}
