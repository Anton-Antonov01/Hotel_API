using Hotel_BL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_BL.Interfaces
{
    public interface IGuestService
    {
        IEnumerable<GuestDTO> GetAllGuests();
        GuestDTO Get(int id);
        void AddGuest(GuestDTO guestDTO);
        void DeleteGuest(int id);
        void UpdateGuest(GuestDTO guestDTO);
    }
}
