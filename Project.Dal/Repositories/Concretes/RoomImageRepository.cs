using Microsoft.EntityFrameworkCore;
using Project.Dal.ContextClasses;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Concretes
{
    public class RoomImageRepository : BaseRepository<RoomImage>, IRoomImageRepository
    {
        public RoomImageRepository(MyContext context) : base(context) { }

        public async Task<List<RoomImage>> GetImagesByRoomIdAsync(int roomId)
        {
            return await _dbSet.Where(ri => ri.RoomId == roomId).ToListAsync();
        }

        public async Task RemoveAllImagesByRoomIdAsync(int roomId)
        {
            var images = await _dbSet.Where(ri => ri.RoomId == roomId).ToListAsync();
            _dbSet.RemoveRange(images);
            await _context.SaveChangesAsync();
        }
    }
}
