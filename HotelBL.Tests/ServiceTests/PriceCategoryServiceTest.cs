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
    public class PriceCategoryServiceTest
    {
        private readonly PriceCategoryService priceCategoryService;
        private readonly CategoryService categoryService;

        private readonly Mock<IWorkUnit> EFWorkUnitMock = new Mock<IWorkUnit>();
        private IMapper Mapper;
        private DataForTests dataForTests;

        public PriceCategoryServiceTest()
        {
            priceCategoryService = new PriceCategoryService(EFWorkUnitMock.Object);
            categoryService = new CategoryService(EFWorkUnitMock.Object);

            Mapper = new MapperConfiguration(
                cfg => {
                    cfg.CreateMap<PriceCategoryDTO, PriceCategory>().ReverseMap();
                    cfg.CreateMap<CategoryDTO, Category>().ReverseMap();
                }
                ).CreateMapper();

            dataForTests = new DataForTests();
        }


        [TestMethod]
        public void GetByIdMethodIsPriceCategoryDTO()
        {
            var PriceCategoryId = 1;

            EFWorkUnitMock.Setup(x => x.PriceCategories.Get(PriceCategoryId)).Returns(dataForTests.PriceCategories.SingleOrDefault(r => r.Id == PriceCategoryId));

            //Act
            var result = priceCategoryService.Get(PriceCategoryId);

            Assert.IsInstanceOfType(result, typeof(PriceCategoryDTO));
        }

        [TestMethod]
        public void GetById_ShouldReturnPriceCategory_WhenPriceCategoryExists()
        {
            //Arrange
            var PriceCategoryId = 1;

            EFWorkUnitMock.Setup(x => x.PriceCategories.Get(PriceCategoryId)).Returns(dataForTests.PriceCategories.SingleOrDefault(r => r.Id == PriceCategoryId));

            //Act
            var result = priceCategoryService.Get(PriceCategoryId);
            var expected = Mapper.Map<PriceCategory, PriceCategoryDTO>(dataForTests.PriceCategories.SingleOrDefault(r => r.Id == PriceCategoryId));

            //Assert
            Assert.AreEqual(expected, result );
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
         "no guest with this id.")]
        public void GetById_ShouldThrowNullReferenceException_WhenPriceCategoryWithIdNotExsists() 
        {
            //Arrange
            var PriceCategoryId = 1111;
            EFWorkUnitMock.Setup(x => x.PriceCategories.Get(PriceCategoryId)).Returns(dataForTests.PriceCategories.SingleOrDefault(r => r.Id == PriceCategoryId));

            //Act
            var PriceCategoryResult = priceCategoryService.Get(PriceCategoryId); 
        }

        [TestMethod]
        public void GetAllMethodIsIEnumerablePriceCategoryDTO()
        {
            //Arrange
            EFWorkUnitMock.Setup(x => x.PriceCategories.GetAll()).Returns(dataForTests.PriceCategories);

            //Act
            var result = priceCategoryService.GetAllPriceCategories();

            //Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<PriceCategoryDTO>));
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllPriceCategories()
        {
            //Arrange
            EFWorkUnitMock.Setup(x => x.PriceCategories.GetAll()).Returns(dataForTests.PriceCategories);

            //Act
            var PriceCategoryResult = priceCategoryService.GetAllPriceCategories();
            IEnumerable<PriceCategoryDTO> expected = Mapper.Map<IEnumerable<PriceCategory>, IEnumerable<PriceCategoryDTO>>(EFWorkUnitMock.Object.PriceCategories.GetAll());

            //Assert
            CollectionAssert.AreEqual(expected.ToList(), PriceCategoryResult.ToList());
        }


        [TestMethod]
        [DataTestMethod]
        [DataRow("2016-03-15", "2017-02-10")]
        [DataRow("2022-03-15", "2023-03-15")]
        [DataRow("2016-03-15", "2017-03-15")]
        public void AddPriceCategory_ShouldAddPriceCategory(string start, string end)
        {
            //Arrange
            var CategoryId = 4;

            var priceCategoryDTO = new PriceCategoryDTO()
            {
                Category = Mapper.Map<Category, CategoryDTO>(dataForTests.Categories.SingleOrDefault(p => p.Id == CategoryId)),
                Price = 100,
                StartDate = Convert.ToDateTime(start),
                EndDate = Convert.ToDateTime(end),
            };


            EFWorkUnitMock.Setup(x => x.PriceCategories.Create(Mapper.Map<PriceCategoryDTO, PriceCategory>(priceCategoryDTO)));
            EFWorkUnitMock.Setup(x => x.PriceCategories.GetAll()).Returns(dataForTests.PriceCategories);

            //Act
            priceCategoryService.AddPriceCategory(priceCategoryDTO);

            //Assert
            EFWorkUnitMock.Verify(x => x.PriceCategories.Create(Mapper.Map<PriceCategoryDTO, PriceCategory>(priceCategoryDTO)));
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
         "Category with CategoryId not exsists")]

        public void AddPriceCategory_ShouldThrowNullReferenceException_WhenCategoryNotExsists()
        {
            //Arrange
            var CategoryId = 1111;

            var priceCategoryDTO = new PriceCategoryDTO()
            {
                Category = Mapper.Map<Category, CategoryDTO>(dataForTests.Categories.SingleOrDefault(p => p.Id == CategoryId)),
                Price = 100,
                StartDate = new DateTime(2016, 3, 15),
                EndDate = new DateTime(2017, 3, 15),
            };


            EFWorkUnitMock.Setup(x => x.PriceCategories.Create(Mapper.Map<PriceCategoryDTO, PriceCategory>(priceCategoryDTO)));
            EFWorkUnitMock.Setup(x => x.PriceCategories.GetAll()).Returns(dataForTests.PriceCategories);


            //Act
            priceCategoryService.AddPriceCategory(priceCategoryDTO);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
         "EndDate < StartDate")]
        public void AddPriceCategory_ShouldThrowNullReferenceException_WhenEndDateLessThenStartDate()
        {
            //Arrange
            var CategoryId = 4;

            var priceCategoryDTO = new PriceCategoryDTO()
            {
                Category = Mapper.Map<Category, CategoryDTO>(dataForTests.Categories.SingleOrDefault(p => p.Id == CategoryId)),
                Price = 100,
                StartDate = new DateTime(2017, 3, 15),
                EndDate = new DateTime(2016, 3, 15),
            };


            EFWorkUnitMock.Setup(x => x.PriceCategories.Create(Mapper.Map<PriceCategoryDTO, PriceCategory>(priceCategoryDTO)));
            EFWorkUnitMock.Setup(x => x.PriceCategories.GetAll()).Returns(dataForTests.PriceCategories);

            //Act
            priceCategoryService.AddPriceCategory(priceCategoryDTO);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
         "there is a PriceCategory for this category for these dates")]
        [DataTestMethod]
        [DataRow("2017-03-15", "2022-03-15")]
        [DataRow("2016-03-15", "2021-03-15")]
        [DataRow("2017-03-15", "2021-03-15")]
        [DataRow("2018-03-15", "2022-03-15")]
        [DataRow("2018-03-15", "2021-03-15")]
        [DataRow("2018-03-15", "2023-03-15")]
        [DataRow("2015-03-15", "2023-03-15")]
        [DataRow("2017-03-15", "2023-03-15")]
        public void AddPriceCategory_ShouldThrowNullReferenceException_WherePriceCategoriesDatesForOneCategoryOverlap(string start, string end)
        {
            //Arrange
            var CategoryId = 4;

            var priceCategoryDTO = new PriceCategoryDTO()
            {
                Category = Mapper.Map<Category, CategoryDTO>(dataForTests.Categories.SingleOrDefault(p => p.Id == CategoryId)),
                Price = 100,
                StartDate = Convert.ToDateTime(start),
                EndDate = Convert.ToDateTime(end)
            };


            EFWorkUnitMock.Setup(x => x.PriceCategories.Create(Mapper.Map<PriceCategoryDTO, PriceCategory>(priceCategoryDTO)));
            EFWorkUnitMock.Setup(x => x.PriceCategories.GetAll()).Returns(dataForTests.PriceCategories);


            //Act
            priceCategoryService.AddPriceCategory(priceCategoryDTO);
        }


        [TestMethod]
        public void DeletePriceCategory_ShouldDeletePriceCategory_WhenPriceCategoryExsists()
        {
            //Arrange
            int PriceCategoryId = 1;
            EFWorkUnitMock.Setup(x => x.PriceCategories.Delete(PriceCategoryId));
            EFWorkUnitMock.Setup(x => x.PriceCategories.Get(PriceCategoryId)).Returns(dataForTests.PriceCategories.SingleOrDefault(r => r.Id == PriceCategoryId));

            //Act
            priceCategoryService.DeletePriceCategory(PriceCategoryId);

            //Assert
            EFWorkUnitMock.Verify(x => x.PriceCategories.Delete(PriceCategoryId));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
         "no PriceCategoty with this id.")]
        public void DeletePriceCategory_ShouldThrowNullReferenceException_WhenPriceCategoryNotExsists()
        {
            //Arrange
            int PriceCategoryId = 1111;
            EFWorkUnitMock.Setup(x => x.PriceCategories.Delete(PriceCategoryId));
            EFWorkUnitMock.Setup(x => x.PriceCategories.Get(PriceCategoryId)).Returns(dataForTests.PriceCategories.SingleOrDefault(r => r.Id == PriceCategoryId));

            //Act
            priceCategoryService.DeletePriceCategory(PriceCategoryId);

            //Assert
            EFWorkUnitMock.Verify(x => x.PriceCategories.Delete(PriceCategoryId));

        }

        [TestMethod]
        [DataTestMethod]
        [DataRow("2017-03-15", "2020-02-10")]
        [DataRow("2017-03-15", "2021-04-20")]
        [DataRow("2016-03-15", "2017-03-15")]
        public void UpdatePriceCategory_ShouldUpdatePriceCategory_WhenPriceCategoryExsists(string start, string end)
        {
            //Arrange
            var CategoryId = 3;
            var PriceCategoryId = 3;
            var priceCategoryDTO = new PriceCategoryDTO()
            {
                Id = PriceCategoryId,
                Category = Mapper.Map<Category, CategoryDTO>(dataForTests.Categories.SingleOrDefault(p => p.Id == CategoryId)),
                Price = 100,
                StartDate = Convert.ToDateTime(start),
                EndDate = Convert.ToDateTime(end)
            };

            EFWorkUnitMock.Setup(x => x.PriceCategories.Update(Mapper.Map<PriceCategoryDTO, PriceCategory>(priceCategoryDTO)));
            EFWorkUnitMock.Setup(x => x.PriceCategories.GetAll()).Returns(dataForTests.PriceCategories);
            EFWorkUnitMock.Setup(x => x.PriceCategories.Get(priceCategoryDTO.Id)).Returns(dataForTests.PriceCategories.SingleOrDefault(r => r.Id == priceCategoryDTO.Id));

            //Act
            priceCategoryService.UpdatePriceCategory(priceCategoryDTO);

            //Assert
            EFWorkUnitMock.Verify(x => x.PriceCategories.Update(Mapper.Map<PriceCategoryDTO, PriceCategory>(priceCategoryDTO)));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
         "no PriceCategory with this id.")]
        public void UpdatePriceCategory_ShouldThrowNullReferenceException_WhenPriceCatNotExsists()
        {
            //Arrange
            var CategoryId = 3;
            var PriceCategoryId = 1111;
            var priceCategoryDTO = new PriceCategoryDTO()
            {
                Id = PriceCategoryId,
                Category = Mapper.Map<Category, CategoryDTO>(dataForTests.Categories.SingleOrDefault(p => p.Id == CategoryId)),
                Price = 100,
                StartDate = new DateTime(2017, 3, 15),
                EndDate = new DateTime(2020, 3, 15),
            };

            EFWorkUnitMock.Setup(x => x.PriceCategories.Update(Mapper.Map<PriceCategoryDTO, PriceCategory>(priceCategoryDTO)));
            EFWorkUnitMock.Setup(x => x.PriceCategories.GetAll()).Returns(dataForTests.PriceCategories);
            EFWorkUnitMock.Setup(x => x.PriceCategories.Get(priceCategoryDTO.Id)).Returns(dataForTests.PriceCategories.SingleOrDefault(r => r.Id == priceCategoryDTO.Id));

            //Act
            priceCategoryService.UpdatePriceCategory(priceCategoryDTO);
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
           "Category is null")]
        public void UpdatePriceCategory_ShouldThrowNullReferenceException_WhenPriceCategoryNotExsists()
        {
            //Arrange
            var CategoryId = 1111;
            var PriceCategoryId = 1;
            var priceCategoryDTO = new PriceCategoryDTO()
            {
                Id = PriceCategoryId,
                Category = Mapper.Map<Category, CategoryDTO>(dataForTests.Categories.SingleOrDefault(p => p.Id == CategoryId)),
                Price = 100,
                StartDate = new DateTime(2017, 3, 15),
                EndDate = new DateTime(2020, 3, 15),
            };

            EFWorkUnitMock.Setup(x => x.PriceCategories.Update(Mapper.Map<PriceCategoryDTO, PriceCategory>(priceCategoryDTO)));
            EFWorkUnitMock.Setup(x => x.PriceCategories.GetAll()).Returns(dataForTests.PriceCategories);
            EFWorkUnitMock.Setup(x => x.PriceCategories.Get(priceCategoryDTO.Id)).Returns(dataForTests.PriceCategories.SingleOrDefault(r => r.Id == priceCategoryDTO.Id));

            //Act
            priceCategoryService.UpdatePriceCategory(priceCategoryDTO);
        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
           "EndData < StartDate")]
        public void UpdatePriceCategory_ShouldThrowArgumentException_WhenEndDateLessThenStartDate()
        {
            //Arrange
            var CategoryId = 1111;
            var PriceCategoryId = 1;
            var priceCategoryDTO = new PriceCategoryDTO()
            {
                Id = PriceCategoryId,
                Category = Mapper.Map<Category, CategoryDTO>(dataForTests.Categories.SingleOrDefault(p => p.Id == CategoryId)),
                Price = 100,
                StartDate = new DateTime(2017, 3, 15),
                EndDate = new DateTime(2016, 3, 15),
            };

            EFWorkUnitMock.Setup(x => x.PriceCategories.Update(Mapper.Map<PriceCategoryDTO, PriceCategory>(priceCategoryDTO)));
            EFWorkUnitMock.Setup(x => x.PriceCategories.GetAll()).Returns(dataForTests.PriceCategories);
            EFWorkUnitMock.Setup(x => x.PriceCategories.Get(priceCategoryDTO.Id)).Returns(dataForTests.PriceCategories.SingleOrDefault(r => r.Id == priceCategoryDTO.Id));

            //Act
            priceCategoryService.UpdatePriceCategory(priceCategoryDTO);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
           "WherePriceCategoriesDatesForOneCategoryOverlap")]
        [DataTestMethod]
        [DataRow("2017-03-15", "2022-03-15")]
        [DataRow("2016-03-15", "2021-03-15")]
        [DataRow("2017-03-15", "2021-03-15")]
        [DataRow("2018-03-15", "2022-03-15")]
        [DataRow("2018-03-15", "2021-03-15")]
        [DataRow("2018-03-15", "2023-03-15")]
        [DataRow("2015-03-15", "2023-03-15")]
        [DataRow("2017-03-15", "2023-03-15")]
        public void UpdatePriceCategory_ShouldThrowArgumentException__WherePriceCategoriesDatesForOneCategoryOverlap(string start, string end)
        {
            //Arrange
            var CategoryId = 4;
            var PriceCategoryId = 1;

            var priceCategoryDTO = new PriceCategoryDTO()
            {
                Id = PriceCategoryId,
                Category = Mapper.Map<Category, CategoryDTO>(dataForTests.Categories.SingleOrDefault(p => p.Id == CategoryId)),
                Price = 100,
                StartDate = Convert.ToDateTime(start),
                EndDate = Convert.ToDateTime(end)
            };


            EFWorkUnitMock.Setup(x => x.PriceCategories.Update(Mapper.Map<PriceCategoryDTO, PriceCategory>(priceCategoryDTO)));
            EFWorkUnitMock.Setup(x => x.PriceCategories.GetAll()).Returns(dataForTests.PriceCategories);

            //Act
            priceCategoryService.UpdatePriceCategory(priceCategoryDTO);
        }

    }
}
