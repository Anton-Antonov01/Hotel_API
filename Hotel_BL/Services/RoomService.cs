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
    public class RoomService : IRoomService
    {

        private IMapper Mapper;
        private IWorkUnit Database { get; set; }

        public RoomService(IWorkUnit database)
        {
            this.Database = database;


            Mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<RoomDTO, Room>().ReverseMap();
                    cfg.CreateMap<CategoryDTO, Category>().ReverseMap();
                }
                ).CreateMapper();

        }

        public IEnumerable<RoomDTO> GetAllRooms()
        {
            List<RoomDTO> rooms = new List<RoomDTO>();
            return Mapper.Map<IEnumerable<Room>,List<RoomDTO>>(Database.Rooms.GetAll());
        }

        public RoomDTO Get(int id)
        {
            var room = Database.Rooms.Get(id);
            if (room == null)
                throw new NullReferenceException();

            return Mapper.Map<Room, RoomDTO>(room);
        }

        public void AddRoom(RoomDTO roomDTO)
        {
            if (roomDTO.RoomCategory == null)
                throw new NullReferenceException();
            if (Database.Rooms.GetAll().Any(r => r.Name == roomDTO.Name))
                throw new ArgumentException();

            Database.Rooms.Create(Mapper.Map<RoomDTO,Room>(roomDTO));
            Database.Save();
        }

        public void DeleteRoom(int id)
        {
            if (Database.Rooms.Get(id) == null)
                throw new NullReferenceException();

            Database.Rooms.Delete(id);
            Database.Save();
        }

        public void UpdateRoom(RoomDTO roomDTO)
        {
            if (roomDTO.RoomCategory == null || Database.Rooms.Get(roomDTO.Id)==null)
                throw new NullReferenceException();
            if (Database.Rooms.GetAll().Any(r => r.Name == roomDTO.Name && r.Id != roomDTO.Id))
                throw new ArgumentException();

            Database.Rooms.Update(Mapper.Map<RoomDTO, Room>(roomDTO));
            Database.Save();
        }
    }
}
