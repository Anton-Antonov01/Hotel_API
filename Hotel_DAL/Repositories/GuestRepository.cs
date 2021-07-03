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
    class GuestRepository: IRepository<Guest>
    {
        HotelContext db;

        public GuestRepository(HotelContext db)
        {
            this.db = db;
        }

        public IEnumerable<Guest> GetAll()
        {
            return db.Guests;
        }

        public Guest Get(int id)
        {
            return db.Guests.Find(id);
        }

        public void Create(Guest guest)
        {
            db.Guests.Add(guest);
        }

        public void Delete(int id)
        {
            Guest guest = Get(id);
            if (guest != null)
                db.Guests.Remove(guest);
        }

        public void Update(Guest guest)
        {
            var toUpdate = db.Guests.FirstOrDefault(m => m.Id == guest.Id);
            if (toUpdate != null)
            {
                toUpdate.Id = guest.Id;
                toUpdate.Name = guest.Name ?? toUpdate.Name;
                toUpdate.Surname = guest.Surname ?? toUpdate.Surname;
                toUpdate.Phone = guest.Phone ?? toUpdate.Phone;
            }

        }

    }
}
