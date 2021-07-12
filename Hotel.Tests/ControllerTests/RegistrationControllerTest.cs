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
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Hotel.Tests.ControllerTests
{
    [TestClass]
    public class RegistrationControllerTest
    {
        IMapper mapper;
        Mock<IGuestService> GuestServiceMock;
        RegistrationController controller;

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

            GuestServiceMock = new Mock<IGuestService>();
            controller = new RegistrationController(GuestServiceMock.Object);

            httpConfiguration = new HttpConfiguration();
            httpRequest = new System.Net.Http.HttpRequestMessage();
            httpRequest.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = httpConfiguration;
        }


        [TestMethod]
        public void RegistrationNewGuestIsHttpResponseMessage()
        {
            GuestServiceMock.Setup(x => x.AddGuest(new GuestDTO()));

            controller.RegistrationNewGuest(httpRequest, new GuestRequest());

            var httpResponse = controller.RegistrationNewGuest(httpRequest, new GuestRequest());
            //var result = httpResponse.Content.ReadAsAsync<GuestModel>();

            Assert.IsInstanceOfType(httpResponse, typeof(HttpResponseMessage));
        }

        [TestMethod]
        public void RegistrationNewGuestShouldRegistNewGuest()
        {
            GuestServiceMock.Setup(x => x.AddGuest(new GuestDTO()));

            controller.RegistrationNewGuest(httpRequest, new GuestRequest());

            GuestServiceMock.Verify(x => x.AddGuest(new GuestDTO()));
        }


        [TestMethod]
        public void RegistrationNewGuest_StatusCodeOK()
        {
            GuestServiceMock.Setup(x => x.AddGuest(new GuestDTO()));

            controller.RegistrationNewGuest(httpRequest, new GuestRequest());

            var httpResponse = controller.RegistrationNewGuest(httpRequest, new GuestRequest());

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);
        }

        [TestMethod]
        public void RegistrationNewGuest_StatusCodeBadRequest()
        {
            GuestServiceMock.Setup(x => x.AddGuest(new GuestDTO())).Throws(new ArgumentException());

            controller.RegistrationNewGuest(httpRequest, new GuestRequest());

            var httpResponse = controller.RegistrationNewGuest(httpRequest, new GuestRequest());

            Assert.AreEqual(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }
    }
}
