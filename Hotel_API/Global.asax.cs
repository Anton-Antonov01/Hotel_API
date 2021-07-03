using Hotel_API.Utils;
using Hotel_BL.Infrastructure;
using Ninject;
using Ninject.Modules;
using Ninject.Web.WebApi;
using Ninject.Web.WebApi.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Hotel_API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);



            NinjectModule roomModule = new RoomModule();
            NinjectModule categoryModule = new CategoryModule();
            NinjectModule guestModule = new GuestModule();
            NinjectModule bookingModule = new BookingModule();
            NinjectModule priceCategoryModule = new PriceCategoryModule();
            NinjectModule profitByMonthModule = new ProfitByMonthModule();
            NinjectModule dependencyModule = new DependencyModule("HotelContext");
            var kernel = new StandardKernel(guestModule,roomModule,categoryModule, bookingModule, priceCategoryModule, profitByMonthModule, dependencyModule);

            kernel.Bind<DefaultFilterProviders>().ToSelf().WithConstructorArgument(GlobalConfiguration.Configuration.Services.GetFilterProviders());
            kernel.Bind<DefaultModelValidatorProviders>().ToConstant(new DefaultModelValidatorProviders(GlobalConfiguration.Configuration.Services.GetModelValidatorProviders()));
            GlobalConfiguration.Configuration.DependencyResolver = new Ninject.Web.WebApi.NinjectDependencyResolver(kernel);


        }
    }
}
