using AutoMapper;
using Hotel_BL.DTO;
using Hotel_BL.Services;
using Hotel_DAL.Entities;
using Hotel_DAL.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HotelBL.Tests.ServiceTests
{
    [TestClass]
    public class GuestServiceTest
    {
        private readonly GuestService guestService;
        private readonly Mock<IWorkUnit> EFWorkUnitMock = new Mock<IWorkUnit>();
        private IMapper Mapper;
        private DataForTests dataForTests;

        public GuestServiceTest()
        {
            guestService = new GuestService(EFWorkUnitMock.Object);

            Mapper = new MapperConfiguration(
                cfg => {
                    cfg.CreateMap<GuestDTO, Guest>().ReverseMap();
                }
                ).CreateMapper();

            dataForTests = new DataForTests();
        }


        [TestMethod]
        public void GetByIdMethodIsGuestDTO()
        {
            var GuestId = 1;

            EFWorkUnitMock.Setup(x => x.Guests.Get(GuestId)).Returns(dataForTests.Guests.SingleOrDefault(r => r.Id == GuestId));

            //Act
            var result = guestService.Get(GuestId);

            Assert.IsInstanceOfType(result, typeof(GuestDTO));
        }


        [TestMethod]
        public void GetById_ShouldReturnGuest_WhenGuestExists()
        {
            //Arrange
            var GuestId = 1;

            EFWorkUnitMock.Setup(x => x.Guests.Get(GuestId)).Returns(dataForTests.Guests.SingleOrDefault(g => g.Id == GuestId));

            //Act
            var result = guestService.Get(GuestId);
            var expected = Mapper.Map<Guest, GuestDTO> (dataForTests.Guests.SingleOrDefault(g => g.Id == GuestId));

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
         "no guest with this id.")]
        public void GetById_ShouldThrowNullReferenceException_WhenGuestWithIdNotExsists()
        {
            //Arrange
            int GuestId = 5;
            EFWorkUnitMock.Setup(x => x.Guests.Get(GuestId)).Returns(dataForTests.Guests.SingleOrDefault(g => g.Id == GuestId));

            //Act
            var guestResult = guestService.Get(GuestId); //если выбрасывается NullReferenceException - тест пройден
        }

        [TestMethod]
        public void GetAllMethodIsIEnumerableGuestDTO()
        {
            //Arrange
            EFWorkUnitMock.Setup(x => x.Guests.GetAll()).Returns(dataForTests.Guests);

            //Act
            var result = guestService.GetAllGuests();

            Assert.IsInstanceOfType(result, typeof(IEnumerable<GuestDTO>));
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllGuest()
        {
            //Arrange
            EFWorkUnitMock.Setup(x => x.Guests.GetAll()).Returns(dataForTests.Guests);

            //Act
            var guestsResult = guestService.GetAllGuests();
            IEnumerable<GuestDTO> expected = Mapper.Map<IEnumerable<Guest>, IEnumerable<GuestDTO>>(EFWorkUnitMock.Object.Guests.GetAll());    

            //Assert
            CollectionAssert.AreEqual(expected.ToList(), guestsResult.ToList());
        }

        [TestMethod]
        public void AddGuest_ShouldAddGuest()
        {
            //Arrange
            var guestDTO = new GuestDTO() { Name = "Segey", Surname = "Antonov", Address = "Address", Phone = "5555" };

            EFWorkUnitMock.Setup(x => x.Guests.Create(Mapper.Map<GuestDTO, Guest>(guestDTO)));
            EFWorkUnitMock.Setup(x => x.Guests.GetAll()).Returns(dataForTests.Guests);

            //Act
            guestService.AddGuest(guestDTO);//Создать екземпляр Guest и перемапить

            //Assert
            EFWorkUnitMock.Verify(x => x.Guests.Create(Mapper.Map<GuestDTO,Guest>(guestDTO)));
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
         "Thera are guest with same Phone")]
        public void AddGuest_ShouldThrowArgumentException_WhenGuestWithSamePhoneExsists()
        {
            //Arrange
            var guestDTO = new GuestDTO() { Name = "Segey", Surname = "Antonov", Address = "Address", Phone = "1111" };

            EFWorkUnitMock.Setup(x => x.Guests.Create(Mapper.Map<GuestDTO,Guest>(guestDTO)));
            EFWorkUnitMock.Setup(x => x.Guests.GetAll()).Returns(dataForTests.Guests);

            //Act
            guestService.AddGuest(guestDTO);
        }


        [TestMethod]
        public void DeleteGuest_ShouldDeleteGuest_WhenGuestExsists()
        {
            //Arrange
            int GuestId = 1; 
            EFWorkUnitMock.Setup(x => x.Guests.Delete(GuestId));
            EFWorkUnitMock.Setup(x => x.Guests.Get(GuestId)).Returns(dataForTests.Guests.SingleOrDefault(g=> g.Id == GuestId)); 

            //Act
            guestService.DeleteGuest(GuestId);//Создать екземпляр Guest и перемапить

            //Assert
            EFWorkUnitMock.Verify(x => x.Guests.Delete(GuestId));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
         "no guest with this id.")]
        public void DeleteGuest_ShouldThrowNullReferenceException_WhenGuestNotExsists()
        {
            //Arrange
            int GuestId = 11;
            EFWorkUnitMock.Setup(x => x.Guests.Delete(GuestId));
            EFWorkUnitMock.Setup(x => x.Guests.Get(GuestId)).Returns(dataForTests.Guests.SingleOrDefault(g => g.Id == GuestId));

            //Act
            guestService.DeleteGuest(GuestId);//Создать екземпляр Guest и перемапить

        }

        [TestMethod]
        public void UpdateGuest_ShouldUpdateGuest_WhenGuestExsists()
        {
            //Arrange 
            var guestDTO = new GuestDTO() { Id = 1, Name = "Anton", Surname = "Antonov", Address = "NewAddress", Phone = "1111" };
            var UpdateGuest = Mapper.Map<GuestDTO,Guest>(guestDTO);

            EFWorkUnitMock.Setup(x => x.Guests.Update(UpdateGuest));
            EFWorkUnitMock.Setup(x => x.Guests.Get(guestDTO.Id)).Returns(dataForTests.Guests.SingleOrDefault(g => g.Id == guestDTO.Id));
            EFWorkUnitMock.Setup(x => x.Guests.GetAll()).Returns(dataForTests.Guests);

            //Action
            guestService.UpdateGuest(guestDTO);

            //Assert
            EFWorkUnitMock.Verify(x => x.Guests.Update(UpdateGuest));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
         "no guest with this id.")]
        public void UpdateGuest_ShouldThrowNullReferenceException_WhenGuestNotExsists()
        {
            //Arrange
            var guestDTO = new GuestDTO() { Id = 11, Name = "Anton", Surname = "Antonov", Address = "NewAddress", Phone = "1111" };
            var UpdateGuest = Mapper.Map<GuestDTO, Guest>(guestDTO);

            EFWorkUnitMock.Setup(x => x.Guests.Update(UpdateGuest));
            EFWorkUnitMock.Setup(x => x.Guests.Get(guestDTO.Id)).Returns(dataForTests.Guests.SingleOrDefault(g => g.Id == guestDTO.Id));
            EFWorkUnitMock.Setup(x => x.Guests.GetAll()).Returns(dataForTests.Guests);

            //Act
            guestService.UpdateGuest(guestDTO);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
           "Thera are guest with same Phone")]
        public void UpdateGuest_ShouldThrowArgumentException_WhenGuestWithSamePhoneExsists()
        {
            //Arrange
            var guestDTO = new GuestDTO() { Id = 1, Name = "Anton", Surname = "Antonov", Address = "NewAddress", Phone = "2222" };
            var UpdateGuest = Mapper.Map<GuestDTO, Guest>(guestDTO);

            EFWorkUnitMock.Setup(x => x.Guests.Update(UpdateGuest));
            EFWorkUnitMock.Setup(x => x.Guests.Get(guestDTO.Id)).Returns(dataForTests.Guests.SingleOrDefault(g => g.Id == guestDTO.Id));
            EFWorkUnitMock.Setup(x => x.Guests.GetAll()).Returns(dataForTests.Guests);

            //Act
            guestService.UpdateGuest(guestDTO);
        }
    }
}
