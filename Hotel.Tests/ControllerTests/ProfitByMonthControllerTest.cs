using AutoMapper;
using Hotel_API.Controllers;
using Hotel_API.Models;
using Hotel_BL.DTO;
using Hotel_BL.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hotel.Tests.ControllerTests
{
    [TestClass]
    public class ProfitByMonthControllerTest
    {
        Mock<IBaseService> BaseServiceMock;
        ProfitByMonthController controller;
        IMapper Mapper;

        public ProfitByMonthControllerTest()
        {
            BaseServiceMock = new Mock<IBaseService>();
            controller = new ProfitByMonthController(BaseServiceMock.Object);

            Mapper = new MapperConfiguration(
                cfg => cfg.CreateMap<ProfitByMonthDTO, ProfitByMonthModel>()
                ).CreateMapper();
        }


        [TestMethod]
        public void GetProfitByMonthsIsIEnumerableProfitByMonthModel()
        {
            BaseServiceMock.Setup(a => a.GetProfitByMonths()).Returns(new List<ProfitByMonthDTO>());

            var result = controller.GetProfitByMonths();

            Assert.IsInstanceOfType(result, typeof(List<ProfitByMonthModel>));
        }

        [TestMethod]
        public void GetProfitByMonthsShouldReturnProfitByMonths()
        {
            BaseServiceMock.Setup(a => a.GetProfitByMonths()).Returns(new List<ProfitByMonthDTO>());

            var result = controller.GetProfitByMonths();
            var expected = BaseServiceMock.Object.GetProfitByMonths();

            CollectionAssert.AreEqual(Mapper.Map<IEnumerable<ProfitByMonthDTO>,List<ProfitByMonthModel>>(expected), result.ToList());
        }

        [TestMethod]
        public void GetProfitOneMonthIsProfitByMonthModel()
        {
            var date1 = new DateTime();

            BaseServiceMock.Setup(a => a.GetProfitByOneMonth(date1)).Returns(new ProfitByMonthDTO());

            var result = controller.GetProfitOneMonth(date1);

            Assert.IsInstanceOfType(result, typeof(ProfitByMonthModel));
        }

        [DataTestMethod]
        [DataRow("2021-2-1", "Февраль 2021")]
        [DataRow("2021-3-1", "Март 2021")]
        [DataRow("2021-4-1", "Апрель 2021")]
        [DataRow("2021-5-1", "Май 2021")]
        [DataRow("2021-6-1", "Июнь 2021")]
        public void GetProfitOneMonthShouldReturnProfitByMonths(string date, string DateLikeString)
        {
            var Date = Convert.ToDateTime(date);
            BaseServiceMock.Setup(a => a.GetProfitByOneMonth(Date)).Returns(new ProfitByMonthDTO() { Month = Date});

            var result = controller.GetProfitOneMonth(Date);
            var expected = DateLikeString;

            Assert.AreEqual(expected, result.Month);
        }
    }
}
