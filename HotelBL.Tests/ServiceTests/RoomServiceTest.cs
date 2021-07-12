using AutoMapper;
using Hotel_BL.DTO;
using Hotel_BL.Services;
using Hotel_DAL.Entities;
using Hotel_DAL.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelBL.Tests.ServiceTests
{
    [TestClass]
    public class RoomServiceTest
    {
        private readonly CategoryService categoryService;
        private readonly RoomService roomService;
        private readonly Mock<IWorkUnit> EFWorkUnitMock = new Mock<IWorkUnit>();
        private IMapper Mapper;
        private DataForTests dataForTests;

        public RoomServiceTest()
        {
            categoryService = new CategoryService(EFWorkUnitMock.Object);
            roomService = new RoomService(EFWorkUnitMock.Object);

            Mapper = new MapperConfiguration(
                cfg => {
                    cfg.CreateMap<RoomDTO, Room>().ReverseMap();
                    cfg.CreateMap<CategoryDTO, Category>().ReverseMap();
                }
                ).CreateMapper();

            dataForTests = new DataForTests();
        }


        [TestMethod]
        public void GetByIdMethodIsRoomDTO()
        {
            var RoomId = 1;
            EFWorkUnitMock.Setup(x => x.Rooms.Get(RoomId)).Returns(dataForTests.Rooms.SingleOrDefault(r => r.Id == RoomId));
            var result = roomService.Get(RoomId);

            Assert.IsInstanceOfType(result, typeof(RoomDTO));
        }

        [TestMethod]
        public void GetById_ShouldReturnRoom_WhenRoomExists()
        {
            //Arrange
            var RoomId = 1;
            EFWorkUnitMock.Setup(x => x.Rooms.Get(RoomId)).Returns(dataForTests.Rooms.SingleOrDefault(r=> r.Id == RoomId));

            //Act
            var result = roomService.Get(RoomId);
            var expected = Mapper.Map<Room, RoomDTO>(dataForTests.Rooms.SingleOrDefault(r => r.Id == RoomId));

            //Assert
            Assert.AreEqual(expected, expected);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
         "no guest with this id.")]
        public void GetById_ShouldThrowNullReferenceException_WhenRoomNotExsists() 
        {
            //Arrange
            int RoomId = 5;
            EFWorkUnitMock.Setup(x => x.Rooms.Get(RoomId)).Returns(dataForTests.Rooms.SingleOrDefault(r => r.Id == RoomId));

            //Act
            var guestResult = roomService.Get(RoomId); 
        }

        [TestMethod]
        public void GetAllMethodIsIEnumerableRoomDTO()
        {
            EFWorkUnitMock.Setup(x => x.Rooms.GetAll()).Returns(dataForTests.Rooms);
            var result = roomService.GetAllRooms();

            Assert.IsInstanceOfType(result, typeof(IEnumerable<RoomDTO>));
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllRooms()
        {
            //Arrange
            EFWorkUnitMock.Setup(x => x.Rooms.GetAll()).Returns(dataForTests.Rooms);

            //Act
            var RoomResult = roomService.GetAllRooms();
            IEnumerable<RoomDTO> expected = Mapper.Map<IEnumerable<Room>, IEnumerable<RoomDTO>>(EFWorkUnitMock.Object.Rooms.GetAll());

            //Assert
            CollectionAssert.AreEqual(expected.ToList(), RoomResult.ToList());
        }

        [TestMethod]
        public void AddRoom_ShouldAddRoom()
        {
            //Arrange
            var Categoryid = 1;
            RoomDTO roomDTO = new RoomDTO { Name = "NewRoom", Active = true, RoomCategory = Mapper.Map<Category, CategoryDTO>(dataForTests.Categories.SingleOrDefault(c => c.Id == Categoryid))};


            EFWorkUnitMock.Setup(x => x.Rooms.Create(Mapper.Map<RoomDTO, Room>(roomDTO)));
            EFWorkUnitMock.Setup(x => x.Rooms.GetAll()).Returns(dataForTests.Rooms);

            //Act
            roomService.AddRoom(roomDTO);

            //Assert
            EFWorkUnitMock.Verify(x => x.Rooms.Create(Mapper.Map<RoomDTO, Room>(roomDTO)));
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
         "There are room with same Name")]
        public void AddRoom_ShouldThrowArgumentException_WhenSameRoomExsists()
        {
            //Arrange
            var Categoryid = 1;
            RoomDTO roomDTO = new RoomDTO { Name = "101", Active = true, RoomCategory = Mapper.Map<Category, CategoryDTO>(dataForTests.Categories.SingleOrDefault(c => c.Id == Categoryid)) };


            EFWorkUnitMock.Setup(x => x.Rooms.Create(Mapper.Map<RoomDTO, Room>(roomDTO)));
            EFWorkUnitMock.Setup(x => x.Rooms.GetAll()).Returns(dataForTests.Rooms);

            //Act
            roomService.AddRoom(roomDTO);
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
           "RoomCategoryNorExsist")]
        public void AddRoom_ShouldThrowArgumentException_WhenRoomCategoryNotExsists()
        {
            //Arrange
            RoomDTO roomDTO = new RoomDTO { Name = "101", Active = true, RoomCategory = null };


            EFWorkUnitMock.Setup(x => x.Rooms.Create(Mapper.Map<RoomDTO, Room>(roomDTO)));
            EFWorkUnitMock.Setup(x => x.Rooms.GetAll()).Returns(dataForTests.Rooms);

            //Act
            roomService.AddRoom(roomDTO);
        }


        [TestMethod]
        public void DeleteRoom_ShouldDeleteRoom_WhenRoomExsists()
        {
            //Arrange
            int RoomId = 1;
            EFWorkUnitMock.Setup(x => x.Rooms.Delete(RoomId));
            EFWorkUnitMock.Setup(x => x.Rooms.Get(RoomId)).Returns(dataForTests.Rooms.SingleOrDefault(r=> r.Id == RoomId));

            //Act
            roomService.DeleteRoom(RoomId);

            //Assert
            EFWorkUnitMock.Verify(x => x.Rooms.Delete(RoomId));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
         "no room with this id.")]
        public void DeleteRoom_ShouldThrowNullReferenceException_WhenRoomNotExsists()
        {
            //Arrange
            int RoomId = 11;
            EFWorkUnitMock.Setup(x => x.Rooms.Delete(RoomId));
            EFWorkUnitMock.Setup(x => x.Rooms.Get(RoomId)).Returns(dataForTests.Rooms.SingleOrDefault(r => r.Id == RoomId));

            //Act
            roomService.DeleteRoom(RoomId);

        }

        [TestMethod]
        public void UpdateRoom_ShouldUpdateRoom_WhenRoomExsists()
        {
            //Arrange
            var Categoryid = 1;
            RoomDTO roomDTO = new RoomDTO {Id = 1, Name = "NewNameRoom", Active = false, RoomCategory = Mapper.Map<Category, CategoryDTO>(dataForTests.Categories.SingleOrDefault(c => c.Id == Categoryid)) };

            EFWorkUnitMock.Setup(x => x.Rooms.Update(Mapper.Map<RoomDTO, Room>(roomDTO)));
            EFWorkUnitMock.Setup(x => x.Rooms.GetAll()).Returns(dataForTests.Rooms);
            EFWorkUnitMock.Setup(x => x.Rooms.Get(roomDTO.Id)).Returns(dataForTests.Rooms.SingleOrDefault(r => r.Id == roomDTO.Id));

            //Act
            roomService.UpdateRoom(roomDTO);

            //Assert
            EFWorkUnitMock.Verify(x => x.Rooms.Update(Mapper.Map<RoomDTO, Room>(roomDTO)));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
         "no room with this id.")]
        public void UpdateRoom_ShouldThrowNullReferenceException_WhenRoomNotExsists()
        {
            //Arrange
            var Categoryid = 1;
            RoomDTO roomDTO = new RoomDTO { Id = 11, Name = "NewNameRoom", Active = false, RoomCategory = Mapper.Map<Category, CategoryDTO>(dataForTests.Categories.SingleOrDefault(c => c.Id == Categoryid)) };

            EFWorkUnitMock.Setup(x => x.Rooms.Update(Mapper.Map<RoomDTO, Room>(roomDTO)));
            EFWorkUnitMock.Setup(x => x.Rooms.GetAll()).Returns(dataForTests.Rooms);
            EFWorkUnitMock.Setup(x => x.Rooms.Get(roomDTO.Id)).Returns(dataForTests.Rooms.SingleOrDefault(r => r.Id == roomDTO.Id));

            //Act
            roomService.UpdateRoom(roomDTO);
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
           "RoomCategory is null")]
        public void UpdateRoom_ShouldThrowNullReferenceException_WhenCategoryNotExsists()
        {
            //Arrange
            RoomDTO roomDTO = new RoomDTO { Id = 11, Name = "NewNameRoom", Active = false, RoomCategory = null };

            EFWorkUnitMock.Setup(x => x.Rooms.Update(Mapper.Map<RoomDTO, Room>(roomDTO)));
            EFWorkUnitMock.Setup(x => x.Rooms.GetAll()).Returns(dataForTests.Rooms);
            EFWorkUnitMock.Setup(x => x.Rooms.Get(roomDTO.Id)).Returns(dataForTests.Rooms.SingleOrDefault(r => r.Id == roomDTO.Id));

            //Act
            roomService.UpdateRoom(roomDTO);
        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
           "Thera are room with same Name")]
        public void UpdateCategory_ShouldThrowArgumentException_WhenSameRoomExsists()
        {
            //Arrange
            var Categoryid = 1;
            RoomDTO roomDTO = new RoomDTO { Id = 1, Name = "202", Active = false, RoomCategory = Mapper.Map<Category, CategoryDTO>(dataForTests.Categories.SingleOrDefault(c => c.Id == Categoryid)) };

            EFWorkUnitMock.Setup(x => x.Rooms.Update(Mapper.Map<RoomDTO, Room>(roomDTO)));
            EFWorkUnitMock.Setup(x => x.Rooms.GetAll()).Returns(dataForTests.Rooms);
            EFWorkUnitMock.Setup(x => x.Rooms.Get(roomDTO.Id)).Returns(dataForTests.Rooms.SingleOrDefault(r => r.Id == roomDTO.Id));

            //Act
            roomService.UpdateRoom(roomDTO);
        }

    }
}
