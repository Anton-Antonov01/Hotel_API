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
    public class GuestController : ApiController
    {
        IGuestService service;
        IMapper Mapper;
        public GuestController(IGuestService service)
        {
            this.service = service;
            Mapper = new MapperConfiguration(
                cfg => {
                    cfg.CreateMap<GuestDTO, GuestModel>().ReverseMap();
                    cfg.CreateMap<GuestRequest, GuestDTO>().ReverseMap();
                }
                ).CreateMapper();
        }

        /// <summary>
        /// Вывод всех гостей
        /// </summary>
        public IEnumerable<GuestModel> Get()
        {
            IEnumerable<GuestDTO> guestsDTO = service.GetAllGuests();

            return Mapper.Map<IEnumerable<GuestDTO>, IEnumerable<GuestModel>>(guestsDTO);
        }

        /// <summary>
        /// Вывод гостя по Id
        /// </summary>
        [ResponseType(typeof(GuestModel))]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            try
            {
                var guestDTO = service.Get(id);
                var guestModel = Mapper.Map<GuestDTO, GuestModel>(guestDTO);
                return request.CreateResponse(HttpStatusCode.OK, guestModel);
            }
            catch(NullReferenceException ex)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Добавление гостя в БД
        /// </summary>
        [HttpPost]
        public HttpResponseMessage Post(HttpRequestMessage request, [FromBody] GuestRequest guest)
        {
            if (!ModelState.IsValid)
                return request.CreateResponse(HttpStatusCode.BadRequest);
            try
            {
                service.AddGuest(Mapper.Map<GuestRequest, GuestDTO>(guest));
                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch(ArgumentException ex)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }


        }

        /// <summary>
        /// Редактирование гостя в БД
        /// </summary>
        public HttpResponseMessage Put(int id, HttpRequestMessage request, [FromBody] GuestRequest guest)
        {
            var guestDTO = Mapper.Map<GuestRequest, GuestDTO>(guest);
            guestDTO.Id = id;

            try
            {
                service.UpdateGuest(guestDTO);
                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch (NullReferenceException ex)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch(ArgumentException ex)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }

        /// <summary>
        /// Удаление гостя из БД
        /// </summary>
        [HttpDelete]
        [ResponseType(typeof(GuestModel))]
        public HttpResponseMessage Delete(HttpRequestMessage request,int id)
        {
            try
            {
                service.DeleteGuest(id);
                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch(NullReferenceException ex)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

    }
}
