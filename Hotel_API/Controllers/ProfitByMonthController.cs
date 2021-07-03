using Hotel_API.Models;
using Hotel_BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Hotel_API.Controllers
{
    public class ProfitByMonthController : ApiController
    {
        IBaseService service;

        public ProfitByMonthController(IBaseService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Вывод заработка по месяцам за все месяцы работы
        /// </summary>
        public IEnumerable<ProfitByMonthModel> GetProfitByMonths()
        {
            var profitBuMonthsDTO = service.GetProfitByMonths();
            List<ProfitByMonthModel> profitByMonthsModel = new List<ProfitByMonthModel>();

            CultureInfo ci = new CultureInfo("ru");
            DateTimeFormatInfo dtfi = ci.DateTimeFormat;

            foreach (var profitByOneMonthDTO in profitBuMonthsDTO)
            {
                ProfitByMonthModel profitByMonthModel = new ProfitByMonthModel();
                profitByMonthModel.Profit = profitByOneMonthDTO.Profit;
                profitByMonthModel.Month = dtfi.GetMonthName(Convert.ToInt32(profitByOneMonthDTO.Month.Month.ToString())) + " " + profitByOneMonthDTO.Month.Year;

                profitByMonthsModel.Add(profitByMonthModel);
            }


            return profitByMonthsModel;
        }

        /// <summary>
        /// Вывод заработка по выбраному месяцу
        /// </summary>
        public ProfitByMonthModel GetProfitOneMonth(DateTime month)
        {
            CultureInfo ci = new CultureInfo("ru");
            DateTimeFormatInfo dtfi = ci.DateTimeFormat;

            var ProfitByMonthDTO = service.GetProfitByOneMonth(month);

            ProfitByMonthModel profitByMonthModel = new ProfitByMonthModel();
            profitByMonthModel.Profit = ProfitByMonthDTO.Profit;
            profitByMonthModel.Month = dtfi.GetMonthName(Convert.ToInt32(ProfitByMonthDTO.Month.Month.ToString())) + " " + ProfitByMonthDTO.Month.Year;

            return profitByMonthModel;
        }
    }
}
