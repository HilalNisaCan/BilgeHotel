using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email); // E-posta ile kullanıcıyı getir
        Task<User> GetUserWithReservationsAsync(int userId); // Kullanıcıyı rezervasyonlarıyla getir
        Task<List<User>> GetUsersWithRoleAsync(UserRole role); // Belirli bir role sahip kullanıcıları getir
        Task<User> GetAsync(Expression<Func<User, bool>> predicate); // ✅ Kullanıcı sorgulamak için

    }
}

