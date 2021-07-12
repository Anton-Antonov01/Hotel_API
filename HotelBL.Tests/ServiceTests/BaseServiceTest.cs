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
    public class BaseServiceTest
    {
        private readonly BaseService baseService;
        private readonly Mock<IWorkUnit> EFWorkUnitMock = new Mock<IWorkUnit>();
        private IMapper Mapper;
        private DataForTests dataForTests;

        public BaseServiceTest()
        {
            baseService = new BaseService(EFWorkUnitMock.Object);

            Mapper = new MapperConfiguration(
                cfg => {
                    cfg.CreateMap<RoomDTO, Room>().ReverseMap();
                    cfg.CreateMap<CategoryDTO, Category>().ReverseMap();
                }
                ).CreateMapper();

            dataForTests = new DataForTests();
        }


        [TestMethod]
        public void ProfitOneMonthMethodIsProfitByMonthDTO()
        {
            EFWorkUnitMock.Setup(x => x.Bookings.GetAll()).Returns(dataForTests.Bookings);
            EFWorkUnitMock.Setup(x => x.PriceCategories.GetAll()).Returns(dataForTests.PriceCategories);

            var result = baseService.GetProfitByOneMonth(Convert.ToDateTime("2021-02-01"));

            Assert.IsInstanceOfType(result, typeof(ProfitByMonthDTO));
        }


        [DataTestMethod]
        [DataRow("2021-03", "800" )]
        [DataRow("2021-04", "14200")]
        [DataRow("2021-05", "19000")]
        public void ProfitOneMonth_ShouldReturnProfitByMonth(string month, string profit)
        {
            EFWorkUnitMock.Setup(x => x.Bookings.GetAll()).Returns(dataForTests.Bookings);
            EFWorkUnitMock.Setup(x => x.PriceCategories.GetAll()).Returns(dataForTests.PriceCategories);

            var result = baseService.GetProfitByOneMonth(Convert.ToDateTime(month));

            var expected = Convert.ToDecimal(profit);

            Assert.AreEqual(expected, result.Profit);
        }


        [TestMethod]
        public void ProfiByMonthshMethodIsIEnumerableProfitByMonthDTO()
        {
            EFWorkUnitMock.Setup(x => x.Bookings.GetAll()).Returns(dataForTests.Bookings);
            EFWorkUnitMock.Setup(x => x.PriceCategories.GetAll()).Returns(dataForTests.PriceCategories);

            var result = baseService.GetProfitByMonths();

            Assert.IsInstanceOfType(result, typeof(IEnumerable<ProfitByMonthDTO>));
        }


        [TestMethod]
        public void ProfitByMonths_ShouldReturnProfitsbyMonths()
        {
            EFWorkUnitMock.Setup(x => x.Bookings.GetAll()).Returns(dataForTests.Bookings);
            EFWorkUnitMock.Setup(x => x.PriceCategories.GetAll()).Returns(dataForTests.PriceCategories);

            List<ProfitByMonthDTO> result = baseService.GetProfitByMonths().ToList();

            var expected = new List<ProfitByMonthDTO>() {
                new ProfitByMonthDTO() {  Month = new DateTime(2021,2,1), Profit = 0m},
                new ProfitByMonthDTO() {  Month = new DateTime(2021,3,1), Profit = 800m},
                new ProfitByMonthDTO() {  Month = new DateTime(2021,4,1), Profit = 14200m},
                new ProfitByMonthDTO() {  Month = new DateTime(2021,5,1), Profit = 19000m},
                new ProfitByMonthDTO() {  Month = new DateTime(2021,6,1), Profit = 0m},
                new ProfitByMonthDTO() {  Month = new DateTime(2021,7,1), Profit = 0m},

            };

            CollectionAssert.AreEqual(expected, result);
        }


    }
}
