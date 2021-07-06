using AutoMapper;
using Hotel_API.Models;
using Hotel_API.RequestModels;
using Hotel_BL.DTO;
using Hotel_BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Hotel_API.Controllers
{
    public class RoomController : ApiController
    {
        private IRoomService roomService;
        private ICategoryService categoryService;

        private IMapper Mapper;
        public RoomController(IRoomService roomService, ICategoryService categoryService)
        {
            this.roomService = roomService;
            this.categoryService = categoryService;

            Mapper = new MapperConfiguration(
                cfg =>{
                    cfg.CreateMap<RoomDTO, RoomModel>().ReverseMap();
                    cfg.CreateMap<CategoryDTO, CategoryModel>().ReverseMap();
                    cfg.CreateMap<RoomRequest, RoomDTO>();
                }).CreateMapper(); 
        }

        /// <summary>
        /// Вывод всех комнат
        /// </summary>
        [HttpGet]
        public IEnumerable<RoomModel> Get()
        {
            var data = roomService.GetAllRooms();
            return Mapper.Map<IEnumerable<RoomDTO>, List<RoomModel>>(data);
        }

        /// <summary>
        /// Вывод одной комнаты по Id
        /// </summary>
        [HttpGet]
        [ResponseType(typeof(RoomModel))]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            try
            {
                var roomDTO = roomService.Get(1);
                var roomModel = Mapper.Map<RoomDTO, RoomModel>(roomDTO);
                return request.CreateResponse(HttpStatusCode.OK, roomModel);
            }
            catch(ArgumentException ex)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }
        }


        /// <summary>
        /// Добавление комнаты в БД
        /// </summary>
        [HttpPost]
        public HttpResponseMessage Post(HttpRequestMessage request, [FromBody] RoomRequest room)
        {
            if (!ModelState.IsValid)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var roomDTO = Mapper.Map<RoomRequest, RoomDTO>(room);
            try
            {
                roomDTO.RoomCategory = categoryService.Get(room.CategoryId);
                roomService.AddRoom(roomDTO);
                return request.CreateResponse(HttpStatusCode.OK);

            }
            catch(ArgumentException ex)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Редактирование комнты в БД
        /// </summary>
        public HttpResponseMessage Put(int id,HttpRequestMessage request, [FromBody] RoomRequest room)
        {
            if(!ModelState.IsValid)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var roomDTO = Mapper.Map<RoomRequest, RoomDTO>(room);
            roomDTO.Id = id;

            try
            {
                roomDTO.RoomCategory = categoryService.Get(room.CategoryId);
                roomService.UpdateRoom(roomDTO);
                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ArgumentException ex)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Удаление комнты из БД
        /// </summary>
        [HttpDelete]
        [ResponseType(typeof(RoomModel))]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            try
            {
                roomService.DeleteRoom(id);
                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ArgumentException ex)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
