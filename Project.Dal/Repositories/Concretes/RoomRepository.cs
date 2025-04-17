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
    public class RoomRepository:BaseRepository<Room>,IRoomRepository
    {
        public RoomRepository(MyContext context):base(context) 
        {

        }



        public async Task<List<Room>> GetAvailableRoomsAsync(DateTime checkInDate, DateTime checkOutDate)
        {
            return await _dbSet.Where(r => !r.Reservations.Any(res =>
                                          (checkInDate >= res.StartDate && checkInDate < res.EndDate) ||
                                          (checkOutDate > res.StartDate && checkOutDate <= res.EndDate) ||
                                          (checkInDate <= res.StartDate && checkOutDate >= res.EndDate)))
                           .ToListAsync();
        }

       

        public async Task<List<Room>> GetRoomsByTypeAsync(RoomType roomType)
        {
            return await _dbSet.Where(r => r.RoomType == roomType).ToListAsync();
        }

        public async Task<Room> GetRoomWithImagesAsync(int roomId)
        {
            return await _dbSet.Include(r => r.RoomImages)
                           .FirstOrDefaultAsync(r => r.Id == roomId);
        }

    }
}
