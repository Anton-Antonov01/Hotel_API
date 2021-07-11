using AutoMapper;
using Hotel_API.Controllers;
using Hotel_API.Models;
using Hotel_API.RequestModels;
using Hotel_BL.DTO;
using Hotel_BL.Interfaces;
using Hotel_BL.Services;
using Hotel_DAL.Entities;
using Hotel_DAL.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace Hotel.Tests.ControllerTests
{
    [TestClass]
    public class RoomContollerTest
    {
        IMapper mapper;
        Mock<IWorkUnit> EFWorkUnitMock;
        Mock<IRoomService> RoomServiceMock;
        Mock<ICategoryService> CategoryServiceMock;

        HttpConfiguration httpConfiguration;
        HttpRequestMessage httpRequest;

        public RoomContollerTest()
        {
            mapper = new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<Room, RoomDTO>();
                cfg.CreateMap<Category, CategoryDTO>();
                cfg.CreateMap<RoomDTO, RoomModel>();
            }).CreateMapper();

            EFWorkUnitMock = new Mock<IWorkUnit>();
            RoomServiceMock = new Mock<IRoomService>();
            CategoryServiceMock = new Mock<ICategoryService>();

            httpConfiguration = new HttpConfiguration();
            httpRequest = new System.Net.Http.HttpRequestMessage();
            httpRequest.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = httpConfiguration;
        }


        [TestMethod]
        public void RoomControllerGetTest()
        {
            int RoomId = 1;

            EFWorkUnitMock.Setup(x => x.Rooms.Get(RoomId)).Returns(new Room());
            RoomServiceMock.Setup(a => a.Get(RoomId)).Returns(new RoomDTO());

            var roomService = new RoomService(EFWorkUnitMock.Object);
            RoomController controller = new RoomController(RoomServiceMock.Object, CategoryServiceMock.Object);

            var httpResponse = controller.Get(httpRequest, RoomId);
            var result = httpResponse.Content.ReadAsAsync<RoomModel>();
            RoomModel expected = mapper.Map<RoomDTO, RoomModel>(roomService.Get(RoomId));
            Assert.AreEqual(expected, result.Result);
        }


        [TestMethod]
        public void RoomControllerGetAllTest()
        {
            EFWorkUnitMock.Setup(x => x.Rooms.GetAll()).Returns(new List<Room>());
            RoomServiceMock.Setup(x => x.GetAllRooms()).Returns(new List<RoomDTO>());

            var roomService = new RoomService(EFWorkUnitMock.Object);
            RoomController controller = new RoomController(RoomServiceMock.Object, CategoryServiceMock.Object);


            var result = controller.Get();

            IEnumerable<RoomModel> expected = mapper.Map<IEnumerable<RoomDTO>, IEnumerable<RoomModel>>(roomService.GetAllRooms());
            CollectionAssert.AreEqual(expected.ToList(), result.ToList());
        }


        [TestMethod]
        public void RoomControllerPostTest()
        {
            EFWorkUnitMock.Setup(x => x.Rooms.Create(new Room()));
            RoomServiceMock.Setup(x => x.AddRoom(new RoomDTO()));

            RoomController controller = new RoomController(RoomServiceMock.Object, CategoryServiceMock.Object);

            controller.Post(httpRequest, new RoomRequest());

            //Похоже суть теста в том, что бы проверить, вызовется ли в контроллере сервис метод с такими же данными ( new RoomDTO() ) 
            RoomServiceMock.Verify(x => x.AddRoom(new RoomDTO()));
        }


        [TestMethod]
        public void RoomControllerDeleteTest()
        {
            int RoomId = 1;
            EFWorkUnitMock.Setup(x => x.Rooms.Delete(RoomId));
            RoomServiceMock.Setup(x => x.DeleteRoom(RoomId));

            RoomController controller = new RoomController(RoomServiceMock.Object, CategoryServiceMock.Object);

            controller.Delete(httpRequest, RoomId);


            RoomServiceMock.Verify(x => x.DeleteRoom(RoomId));
        }

        [TestMethod]
        public void RoomControllerUpdateTest()
        {
            int RoomId = 1;
            EFWorkUnitMock.Setup(x => x.Rooms.Update(new Room()));
            RoomServiceMock.Setup(x => x.UpdateRoom(new RoomDTO()));

            RoomController controller = new RoomController(RoomServiceMock.Object, CategoryServiceMock.Object);

            controller.Put(RoomId, httpRequest, new RoomRequest());

            //Похоже суть теста в том, что бы проверить, вызовется ли в контроллере сервис метод с такими же данными 
            RoomServiceMock.Verify(x => x.UpdateRoom(new RoomDTO() { Id = 1 }));
        }
    }
}
