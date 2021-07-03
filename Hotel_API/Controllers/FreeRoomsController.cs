using AutoMapper;
using Hotel_API.Models;
using Hotel_BL.DTO;
using Hotel_BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Hotel_API.Controllers
{
    public class FreeRoomsController : ApiController
    {
        IBaseService service;
        IMapper Mapper;

        public FreeRoomsController(IBaseService service)
        {
            this.service = service;

            Mapper = new MapperConfiguration(
                cfg => {
                    cfg.CreateMap<RoomDTO, RoomModel>();
                    cfg.CreateMap<CategoryDTO, CategoryModel>();                   
                }).CreateMapper();
        }


        /// <summary>
        /// Вывод всех свободных комнат на данный момент
        /// </summary>
        [HttpGet]
        public IEnumerable<RoomModel> FreeRoomsNow()
        {
            return Mapper.Map<IEnumerable<RoomDTO>, IEnumerable<RoomModel>>(service.FreeRoomsByDate(DateTime.Now));
        }

        /// <summary>
        /// Вывод всех свободных комнат на дату
        /// </summary>
        [HttpGet]
        public IEnumerable<RoomModel> FreeRoomsByDate(DateTime date)
        {
            return Mapper.Map<IEnumerable<RoomDTO>, IEnumerable<RoomModel>>(service.FreeRoomsByDate(date));
        }

        /// <summary>
        /// Вывод всех свободных комнат на диапазон дат
        /// </summary>
        [HttpGet]
        public IEnumerable<RoomModel> FreeRoomsByDateRange(DateTime FirstDate, DateTime SecondDate)
        {
            return Mapper.Map<IEnumerable<RoomDTO>, IEnumerable<RoomModel>>(service.FreeRoomsByDateRange(FirstDate, SecondDate));
        }
    }
}
