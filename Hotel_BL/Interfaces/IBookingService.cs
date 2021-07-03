using Hotel_BL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_BL.Interfaces
{
    public interface IBookingService
    {
        BookingDTO Get(int id);
        IEnumerable<BookingDTO> GetAllBookings();
        void AddBooking(BookingDTO bookingDTO);
        void DeleteBooking(int id);
        void UpdateBooking(BookingDTO bookingDTO);

    }
}
