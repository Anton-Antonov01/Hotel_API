using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotel_API.Models
{
    public class BookingModel
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime EnterDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public bool Set { get; set; }
        public virtual GuestModel guest { get; set; }
        public virtual RoomModel room { get; set; }


        public override bool Equals(object obj)
        {
            if (obj is BookingModel)
            {
                var objCM = obj as BookingModel;
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