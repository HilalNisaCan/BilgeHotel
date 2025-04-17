using Microsoft.EntityFrameworkCore;
using Project.Dal.ContextClasses;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Concretes
{
    public class RoomTypePriceRepository : BaseRepository<RoomTypePrice>, IRoomTypePriceRepository
    {
        public RoomTypePriceRepository(MyContext context) : base(context)
        {
        }

        public  async Task<RoomTypePrice?> GetByRoomTypeAsync(RoomType roomType)
        {
           return await _context.RoomTypePrices.FirstOrDefaultAsync(x=> x.RoomType == roomType && x.Status==DataStatus.Inserted);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }


    }
}
