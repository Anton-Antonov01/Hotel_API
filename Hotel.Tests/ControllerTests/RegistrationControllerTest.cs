using AutoMapper;
using Hotel_API.Controllers;
using Hotel_API.Models;
using Hotel_API.RequestModels;
using Hotel_BL.DTO;
using Hotel_BL.Interfaces;
using Hotel_DAL.Entities;
using Hotel_DAL.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net.Http;
using System.Web.Http;

namespace Hotel.Tests.ControllerTests
{
    [TestClass]
    public class RegistrationControllerTest
    {
        IMapper mapper;
        Mock<IWorkUnit> EFWorkUnitMock;
        Mock<IGuestService> GuestServiceMock;

        HttpConfiguration httpConfiguration;
        HttpRequestMessage httpRequest;

        public RegistrationControllerTest()
        {
            mapper = new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<Guest, GuestDTO>();
                cfg.CreateMap<GuestDTO, GuestModel>();
            }).CreateMapper();

            EFWorkUnitMock = new Mock<IWorkUnit>();
            GuestServiceMock = new Mock<IGuestService>();


            httpConfiguration = new HttpConfiguration();
            httpRequest = new System.Net.Http.HttpRequestMessage();
            httpRequest.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = httpConfiguration;
        }


        [TestMethod]
        public void RegistrationController_RegistrationNewGuestTest()
        {
            EFWorkUnitMock.Setup(x => x.Guests.Create(new Guest()));
            GuestServiceMock.Setup(x => x.AddGuest(new GuestDTO()));

            RegistrationController controller = new RegistrationController(GuestServiceMock.Object);

            controller.RegistrationNewGuest(httpRequest, new GuestRequest());

            //Похоже суть теста в том, что бы проверить, вызовется ли в контроллере сервис метод с такими же данными ( new GuestDTO() ) 
            GuestServiceMock.Verify(x => x.AddGuest(new GuestDTO()));
        }
    }
}
