using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Enums
{
    public enum RoomStatus
    {
       
        Available=0,   // Boş
        Occupied = 1,    // Dolu 
        Cleaning=2,    // Temizlikte
        Maintenance =3 // Bakımda
    }
}
