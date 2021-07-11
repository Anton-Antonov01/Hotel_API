using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_BL.DTO
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime EnterDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public bool Set { get; set; }
        public  GuestDTO guest { get; set; }
        public  RoomDTO room { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is BookingDTO)
            {
                var objCM = obj as BookingDTO;
                return this.Id == objCM.Id
                    && this.BookingDate == objCM.BookingDate
                    && this.EnterDate == objCM.EnterDate
                    && this.LeaveDate == objCM.LeaveDate
                    && this.Set == objCM.Set; //Добавить сравнение комнаты и гостя
            }
            else
            {
                return base.Equals(obj);
            }
        }
    }
}
