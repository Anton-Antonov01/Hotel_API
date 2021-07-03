using Hotel_BL.Interfaces;
using Hotel_BL.Services;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotel_API.Utils
{
    public class ProfitByMonthModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBaseService>().To<BaseService>();
        }
    }
}