using Microsoft.EntityFrameworkCore;
using Project.Dal.ContextClasses;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Concretes
{
    public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(MyContext context) : base(context)
        {
        }

        // Odaya ait tüm rezervasyonları getir
        public async Task<List<Reservation>> GetReservationsByRoomIdAsync(int roomId)
        {
            return await _dbSet.Where(r => r.RoomId == roomId).ToListAsync();
        }

        // Müşteriye ait tüm rezervasyonları getir
        public async Task<List<Reservation>> GetReservationsByCustomerIdAsync(int customerId)
        {
            return await _dbSet.Where(r => r.CustomerId == customerId).ToListAsync();
        }

        // Tek bir rezervasyonu getir
        public async Task<Reservation> GetByReservationIdAsync(int reservationId)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.Id == reservationId);
        }

        public new async Task<List<Reservation>> GetAllWithIncludeAsync(
      Expression<Func<Reservation, bool>> predicate,
      Func<IQueryable<Reservation>, IQueryable<Reservation>> include)
        {
            IQueryable<Reservation> query = _dbSet;

            if (include != null)
                query = include(query);

            if (predicate != null)
                query = query.Where(predicate);

            return await query.ToListAsync();
        }

        public async Task<bool> DeleteReservationAsync(int id)
        {

            Reservation? reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return false;

            _context.Reservations.Remove(reservation);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
