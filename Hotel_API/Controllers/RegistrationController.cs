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
    public class RegistrationController : ApiController
    {
        IGuestService service;
        IMapper Mapper;

        public RegistrationController(IGuestService service)
        {
            this.service = service;
            Mapper = new MapperConfiguration(
                cfg => {
                    cfg.CreateMap<GuestRequest, GuestDTO>();
                }
                ).CreateMapper();
        }
        
        /// <summary>
        /// Регистрация пользователя 
        /// </summary>
        [HttpPost]
        public HttpResponseMessage RegistrationNewGuest(HttpRequestMessage request ,[FromBody] GuestRequest guest) 
        {
            if (!ModelState.IsValid)
                return request.CreateResponse(HttpStatusCode.BadRequest);
            try
            {
                service.AddGuest(Mapper.Map<GuestRequest, GuestDTO>(guest));
                return request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
