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
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace Hotel.Tests.ControllerTests
{
    [TestClass]
    public class PriceCategoryControllerTest
    {
        IMapper mapper;
        Mock<IWorkUnit> EFWorkUnitMock;
        Mock<IPriceCategoryService> PriceCategoryServiceMock;
        Mock<ICategoryService> CategoryServiceMock;

        HttpConfiguration httpConfiguration;
        HttpRequestMessage httpRequest;

        public PriceCategoryControllerTest()
        {
            mapper = new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<PriceCategory, PriceCategoryDTO>();
                cfg.CreateMap<Category, CategoryDTO>();
                cfg.CreateMap<PriceCategoryDTO, PriceCategoryModel>();
            }).CreateMapper();

            EFWorkUnitMock = new Mock<IWorkUnit>();
            PriceCategoryServiceMock = new Mock<IPriceCategoryService>();
            CategoryServiceMock = new Mock<ICategoryService>();

            httpConfiguration = new HttpConfiguration();
            httpRequest = new System.Net.Http.HttpRequestMessage();
            httpRequest.Properties[System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey] = httpConfiguration;
        }


        [TestMethod]
        public void PriceCateogoryControllerGetTest()
        {
            int id = 1;

            EFWorkUnitMock.Setup(x => x.PriceCategories.Get(id)).Returns(new PriceCategory());
            PriceCategoryServiceMock.Setup(a => a.Get(id)).Returns(new PriceCategoryDTO());

            PriceCategoryController controller = new PriceCategoryController(PriceCategoryServiceMock.Object, CategoryServiceMock.Object);

            var httpResponse = controller.Get(httpRequest, id);
            var result = httpResponse.Content.ReadAsAsync<PriceCategoryModel>();
            PriceCategoryModel expected = mapper.Map<PriceCategoryDTO, PriceCategoryModel>(PriceCategoryServiceMock.Object.Get(id));
            Assert.AreEqual(expected, result.Result);
        }


        [TestMethod]
        public void PriceCateogoryControllerGetAllTest()
        {
            EFWorkUnitMock.Setup(x => x.PriceCategories.GetAll()).Returns(new List<PriceCategory>());
            PriceCategoryServiceMock.Setup(x => x.GetAllPriceCategories()).Returns(new List<PriceCategoryDTO>());

            PriceCategoryController controller = new PriceCategoryController(PriceCategoryServiceMock.Object, CategoryServiceMock.Object);


            var result = controller.Get();

            IEnumerable<PriceCategoryModel> expected = mapper.Map<IEnumerable<PriceCategoryDTO>, IEnumerable<PriceCategoryModel>>(PriceCategoryServiceMock.Object.GetAllPriceCategories());
            CollectionAssert.AreEqual(expected.ToList(), result.ToList());
        }


        [TestMethod]
        public void PriceCateogoryControllerPostTest()
        {
            EFWorkUnitMock.Setup(x => x.PriceCategories.Create(new PriceCategory()));
            PriceCategoryServiceMock.Setup(x => x.AddPriceCategory(new PriceCategoryDTO()));

            PriceCategoryController controller = new PriceCategoryController(PriceCategoryServiceMock.Object, CategoryServiceMock.Object);

            controller.Post(httpRequest, new PriceCategoryRequest());

            //Похоже суть теста в том, что бы проверить, вызовется ли в контроллере сервис метод с такими же данными ( new RoomDTO() ) 
            PriceCategoryServiceMock.Verify(x => x.AddPriceCategory(new PriceCategoryDTO()));
        }


        [TestMethod]
        public void PriceCateogoryControllerDeleteTest()
        {
            int id = 1;
            EFWorkUnitMock.Setup(x => x.Rooms.Delete(id));
            PriceCategoryServiceMock.Setup(x => x.DeletePriceCategory(id));

            PriceCategoryController controller = new PriceCategoryController(PriceCategoryServiceMock.Object, CategoryServiceMock.Object);

            controller.Delete(httpRequest, id);


            PriceCategoryServiceMock.Verify(x => x.DeletePriceCategory(id));
        }

        [TestMethod]
        public void PriceCateogoryControllerUpdateTest()
        {
            int RoomId = 1;
            EFWorkUnitMock.Setup(x => x.PriceCategories.Update(new PriceCategory()));
            PriceCategoryServiceMock.Setup(x => x.UpdatePriceCategory(new PriceCategoryDTO()));

            PriceCategoryController controller = new PriceCategoryController(PriceCategoryServiceMock.Object, CategoryServiceMock.Object);

            controller.Put(httpRequest, RoomId, new PriceCategoryRequest());

            //Похоже суть теста в том, что бы проверить, вызовется ли в контроллере сервис метод с такими же данными 
            PriceCategoryServiceMock.Verify(x => x.UpdatePriceCategory(new PriceCategoryDTO() { Id = 1 }));
        }
    }
}
