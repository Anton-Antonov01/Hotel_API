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
    public class CategoryServiceTest
    {

        private readonly CategoryService categoryService;
        private readonly Mock<IWorkUnit> EFWorkUnitMock = new Mock<IWorkUnit>();
        private IMapper Mapper;
        private DataForTests dataForTests;
        public CategoryServiceTest()
        {
            categoryService = new CategoryService(EFWorkUnitMock.Object);

            Mapper = new MapperConfiguration(
                cfg => {
                    cfg.CreateMap<CategoryDTO, Category>().ReverseMap();
                }
                ).CreateMapper();

            dataForTests = new DataForTests();
        }



        [TestMethod]
        public void GetByIdMethodIsCategoryDTO()
        {
            var CategoryId = 1;
            EFWorkUnitMock.Setup(x => x.Categories.Get(CategoryId)).Returns(dataForTests.Categories.SingleOrDefault(c => c.Id == CategoryId));


            //Act
            var result = categoryService.Get(CategoryId);

            Assert.IsInstanceOfType(result, typeof(CategoryDTO));
        }

        [TestMethod]
        public void GetById_ShouldReturnCategory_WhenCategoryExists()
        {
            //Arrange
            var CategoryId = 1;
            EFWorkUnitMock.Setup(x => x.Categories.Get(CategoryId)).Returns(dataForTests.Categories.SingleOrDefault(c=> c.Id == CategoryId));


            //Act
            var result = categoryService.Get(CategoryId);
            var expected = Mapper.Map<Category, CategoryDTO>(dataForTests.Categories.SingleOrDefault(c => c.Id == CategoryId));

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
         "no guest with this id.")]
        public void GetById_ShouldThrowNullReferenceException_WhenCategoryWithIdNotExsists() 
        {
            //Arrange
            int Categoryid = 5;
            EFWorkUnitMock.Setup(x => x.Categories.Get(Categoryid)).Returns(dataForTests.Categories.SingleOrDefault(g => g.Id == Categoryid));

            //Act
            var guestResult = categoryService.Get(Categoryid); //если выбрасывается NullReferenceException - тест пройден
        }


        [TestMethod]
        public void GetAllMethodIsIEnumerableCategoryDTO()
        {
            //Arrange
            EFWorkUnitMock.Setup(x => x.Categories.GetAll()).Returns(dataForTests.Categories);

            //Act
            var result = categoryService.GetAllCategories();

            Assert.IsInstanceOfType(result, typeof(IEnumerable<CategoryDTO>));
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllCategories()
        {
            //Arrange
            EFWorkUnitMock.Setup(x => x.Categories.GetAll()).Returns(dataForTests.Categories);

            //Act
            var result = categoryService.GetAllCategories();
            IEnumerable<CategoryDTO> expected = Mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(EFWorkUnitMock.Object.Categories.GetAll());

            //Assert
            CollectionAssert.AreEqual(expected.ToList(), result.ToList());
        }

        [TestMethod]
        public void AddCategory_ShouldAddCategory()
        {
            //Arrange
            var categoryDTO = new CategoryDTO() { Name = "NewCategory", Bed = 1, };
            EFWorkUnitMock.Setup(x => x.Categories.Create(Mapper.Map<CategoryDTO, Category>(categoryDTO)));
            EFWorkUnitMock.Setup(x => x.Categories.GetAll()).Returns(dataForTests.Categories);

            //Act
            categoryService.AddCategory(categoryDTO);//Создать екземпляр Guest и перемапить

            //Assert
            EFWorkUnitMock.Verify(x => x.Categories.Create(Mapper.Map<CategoryDTO, Category>(categoryDTO)));
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
         "There are category with same Name and Bed")]
        public void AddCategory_ShouldThrowArgumentException_WhenSameCategoryExsists()
        {
            //Arrange
            var categoryDTO = new CategoryDTO() { Name = "Standard", Bed = 1, };

            EFWorkUnitMock.Setup(x => x.Categories.Create(Mapper.Map<CategoryDTO, Category>(categoryDTO)));
            EFWorkUnitMock.Setup(x => x.Categories.GetAll()).Returns(dataForTests.Categories);

            //Act
            categoryService.AddCategory(categoryDTO);
        }


        [TestMethod]
        public void DeleteCategory_ShouldDeleteCategory_WhenCategoryExsists()
        {
            //Arrange
            int CategoryId = 1;
            EFWorkUnitMock.Setup(x => x.Categories.Delete(CategoryId));
            EFWorkUnitMock.Setup(x => x.Categories.Get(CategoryId)).Returns(dataForTests.Categories.SingleOrDefault(c => c.Id == CategoryId)); 

            //Act
            categoryService.DeleteCategory(CategoryId);

            //Assert
            EFWorkUnitMock.Verify(x => x.Categories.Delete(CategoryId));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
         "no guest with this id.")]
        public void DeleteCategory_ShouldThrowNullReferenceException_WhenCategoryNotExsists()
        {
            //Arrange
            int CategoryId = 111;
            EFWorkUnitMock.Setup(x => x.Categories.Delete(CategoryId));
            EFWorkUnitMock.Setup(x => x.Categories.Get(CategoryId)).Returns(dataForTests.Categories.SingleOrDefault(c => c.Id == CategoryId));

            //Act
            categoryService.DeleteCategory(CategoryId);
        }

        [TestMethod]
        public void UpdateCategory_ShouldUpdateCategory_WhenCategoryExsists()
        {
            //Arrange 
            var categoryDTO = new CategoryDTO() { Id = 1, Name = "NewName", Bed = 1, };


            EFWorkUnitMock.Setup(x => x.Categories.Update(Mapper.Map<CategoryDTO,Category>(categoryDTO)));
            EFWorkUnitMock.Setup(x => x.Categories.Get(categoryDTO.Id)).Returns(dataForTests.Categories.SingleOrDefault(c => c.Id == categoryDTO.Id));
            EFWorkUnitMock.Setup(x => x.Categories.GetAll()).Returns(dataForTests.Categories);

            //Action
            categoryService.UpdateCategory(categoryDTO);

            //Assert
            EFWorkUnitMock.Verify(x => x.Categories.Update(Mapper.Map<CategoryDTO, Category>(categoryDTO)));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
         "no category with this id.")]
        public void UpdateCategory_ShouldThrowNullReferenceException_WhenCategoryNotExsists()
        {
            //Arrange
            var categoryDTO = new CategoryDTO() { Id = 11, Name = "NewName", Bed = 1, };

            EFWorkUnitMock.Setup(x => x.Categories.Update(Mapper.Map<CategoryDTO, Category>(categoryDTO)));
            EFWorkUnitMock.Setup(x => x.Categories.Get(categoryDTO.Id)).Returns(dataForTests.Categories.SingleOrDefault(c => c.Id == categoryDTO.Id));
            EFWorkUnitMock.Setup(x => x.Categories.GetAll()).Returns(dataForTests.Categories);

            //Act
            categoryService.UpdateCategory(categoryDTO);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
           "Thera are Category with same Name and Bed")]
        public void UpdateCategory_ShouldThrowArgumentException_WhenSameCategoryExsists()
        {
            //Arrange
            var categoryDTO = new CategoryDTO() { Id = 2, Name = "Standard", Bed = 1, };

            EFWorkUnitMock.Setup(x => x.Categories.Update(Mapper.Map<CategoryDTO, Category>(categoryDTO)));
            EFWorkUnitMock.Setup(x => x.Categories.Get(categoryDTO.Id)).Returns(dataForTests.Categories.SingleOrDefault(c => c.Id == categoryDTO.Id));
            EFWorkUnitMock.Setup(x => x.Categories.GetAll()).Returns(dataForTests.Categories);

            //Act
            categoryService.UpdateCategory(categoryDTO);
        }
    }
}
