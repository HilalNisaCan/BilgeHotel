using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IReservationRepository:IRepository<Reservation>
    {
        Task<List<Reservation>> GetReservationsByRoomIdAsync(int roomId); // Odaya ait rezervasyonları getir

        Task<List<Reservation>> GetReservationsByCustomerIdAsync(int customerId); // Müşteriye ait rezervasyonları getir

        Task<Reservation> GetByReservationIdAsync(int reservationId); // Tek bir rezervasyonu getir
        new Task UpdateAsync(Reservation reservation);
        new Task<List<Reservation>> GetAllWithIncludeAsync(
    Expression<Func<Reservation, bool>> predicate,
    Func<IQueryable<Reservation>, IQueryable<Reservation>> include);


    }
}

