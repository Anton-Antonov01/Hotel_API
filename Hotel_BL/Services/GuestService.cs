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
    public class GuestService : IGuestService
    {
        IWorkUnit Database { get; set; }
        IMapper Mapper;

        public GuestService(IWorkUnit database)
        {
            Database = database;
            Mapper = new MapperConfiguration(
                cfg => cfg.CreateMap<Guest, GuestDTO>().ReverseMap()
                ).CreateMapper();
        }

        public IEnumerable<GuestDTO> GetAllGuests()
        {
            var guests = Database.Guests.GetAll();

            return Mapper.Map<IEnumerable<Guest>, IEnumerable<GuestDTO>>(guests);
        }

        public GuestDTO Get(int id)
        {
            var guest = Database.Guests.Get(id);
            if (guest == null)
                throw new NullReferenceException();
            
            return Mapper.Map<Guest, GuestDTO>(guest);
        }

        public void AddGuest(GuestDTO guestDTO)
        {
            if (Database.Guests.GetAll().Any(g => g.Phone == guestDTO.Phone))
                throw new ArgumentException();

            Database.Guests.Create(Mapper.Map<GuestDTO, Guest>(guestDTO));
            Database.Save();
        }

        public void DeleteGuest(int id)
        {
            if (Database.Guests.Get(id) == null)
                throw new NullReferenceException();

            Database.Guests.Delete(id);
            Database.Save();
        }

        public void UpdateGuest(GuestDTO guestDTO)
        {
            var guest = Database.Guests.Get(guestDTO.Id);
            if (guest == null)
                throw new NullReferenceException();
            if (Database.Guests.GetAll().Any(g => g.Phone == guestDTO.Phone && g.Id != guestDTO.Id))
                throw new ArgumentException();

            Database.Guests.Update(Mapper.Map<GuestDTO, Guest>(guestDTO));
            Database.Save();
        }
    }
}
