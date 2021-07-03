using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotel_API.Models
{
    public class ProfitByMonthModel
    {
        public int Id { get; set; }
        public decimal Profit { get; set; }
        public string Month { get; set; }
    }
}