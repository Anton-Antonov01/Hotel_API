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
using System.Web.Http.Description;
using Hotel_API.RequestModels;

namespace Hotel_API.Controllers
{
    public class BookingController : ApiController
    {
        IMapper Mapper;
        IBookingService bookingService;
        IRoomService roomService;
        IGuestService guestService;

        public BookingController(IBookingService bookingService, IRoomService roomService, IGuestService guestService)
        {
            this.bookingService = bookingService;
            this.roomService = roomService; 
            this.guestService = guestService;


            Mapper = new MapperConfiguration(
                cfg => {
                    cfg.CreateMap<BookingModel, BookingDTO>().ReverseMap();
                    cfg.CreateMap<RoomModel, RoomDTO>().ReverseMap();
                    cfg.CreateMap<GuestModel, GuestDTO>().ReverseMap();
                    cfg.CreateMap<CategoryModel, CategoryDTO>().ReverseMap();
                    cfg.CreateMap<BookingRequest, BookingDTO>().ReverseMap();
                })
                .CreateMapper();
        }


        /// <summary>
        /// Вывод всех бронирований
        /// </summary>
        public IEnumerable<BookingModel> Get()
        {
            var booking = bookingService.GetAllBookings();

            return Mapper.Map<IEnumerable<BookingDTO>, IEnumerable<BookingModel>>(booking);
        }



        /// <summary>
        /// Вывод одного бронирования по Id из БД
        /// </summary>
        [ResponseType(typeof(BookingModel))]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            try
            {
                var bookingModel = Mapper.Map<BookingDTO, BookingModel>(bookingService.Get(id));
                return request.CreateResponse(HttpStatusCode.OK, bookingModel);
            }
            catch (ArgumentException ex)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }
           
        }

        /// <summary>
        /// Добавление бронирования в БД
        /// </summary>
        public HttpResponseMessage Post(HttpRequestMessage request ,[FromBody]BookingRequest booking)
        {
            if (!ModelState.IsValid)
                return request.CreateResponse(HttpStatusCode.BadRequest);

            BookingDTO bookingDTO = Mapper.Map<BookingRequest,BookingDTO>(booking);

            try
            {
                var room = roomService.Get(booking.RoomId);
                var guest = guestService.Get(booking.GuestId);

                bookingDTO.room = room;
                bookingDTO.guest = guest;

                bookingService.AddBooking(bookingDTO);
                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch(ArgumentException ex)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Редактирование бронирования
        /// </summary>
        public HttpResponseMessage Put(HttpRequestMessage request, int id, [FromBody] BookingRequest booking)
        {
            if (!ModelState.IsValid)
                return request.CreateResponse(HttpStatusCode.BadRequest);

            BookingDTO bookingDTO = Mapper.Map<BookingRequest, BookingDTO>(booking);

            try
            {
                var room = roomService.Get(booking.RoomId);
                var guest = guestService.Get(booking.GuestId);

                bookingDTO.room = room;
                bookingDTO.guest = guest;
                bookingDTO.Id = id;

                bookingService.UpdateBooking(bookingDTO);
                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ArgumentException ex)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Удаление бронирования из БД
        /// </summary>
        [HttpDelete]
        [ResponseType(typeof(BookingModel))]
        public HttpResponseMessage Delete(HttpRequestMessage request,int id)
        {
            try
            {
                bookingService.DeleteBooking(id);
                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch(ArgumentException ex)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
