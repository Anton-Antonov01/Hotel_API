using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_BL.DTO
{
    public class ProfitByMonthDTO
    {
        public decimal Profit { get; set; }
        public DateTime Month { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is ProfitByMonthDTO)
            {
                var objPDTO = obj as ProfitByMonthDTO;
                return this.Profit == objPDTO.Profit
                    && this.Month == objPDTO.Month;
            }
            else
            {
                return base.Equals(obj);
            }
        }
    }
}
