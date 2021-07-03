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

namespace Hotel_API.Controllers
{
    public class RoomBookingController : ApiController
    {
        IMapper Mapper;
        IBookingService bookingservice;
        IRoomService roomService;
        IGuestService guestService;
        IBaseService baseService;


        public RoomBookingController(IBookingService service, IRoomService roomService, IGuestService guestService, IBaseService baseService)
        {
            this.bookingservice = service;
            this.roomService = roomService;
            this.guestService = guestService;
            this.baseService = baseService;


            Mapper = new MapperConfiguration(
                cfg => {
                    cfg.CreateMap<BookingModel, BookingDTO>().ReverseMap();
                    cfg.CreateMap<RoomModel, RoomDTO>().ReverseMap();
                    cfg.CreateMap<GuestModel, GuestDTO>().ReverseMap();
                    cfg.CreateMap<CategoryModel, CategoryDTO>().ReverseMap();
                    cfg.CreateMap<RoomBookingRequest, BookingDTO>().ReverseMap();
                })
                .CreateMapper();

        }


        /// <summary>
        /// Бронирование комнаты
        /// </summary>
        public HttpResponseMessage DoBooking(HttpRequestMessage request, [FromBody] RoomBookingRequest roomBooking)
        {
            BookingDTO bookingDTO = Mapper.Map<RoomBookingRequest, BookingDTO>(roomBooking);
            try
            {
                var room = roomService.Get(roomBooking.RoomId);
                var guest = guestService.Get(roomBooking.GuestId);

                bookingDTO.room = room;
                bookingDTO.guest = guest;
                //bookingDTO.BookingDate = DateTime.Now;
                bookingDTO.LeaveDate = roomBooking.EnterDate.AddDays(roomBooking.NumberOfDays);

                baseService.DoBooking(bookingDTO);
                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
