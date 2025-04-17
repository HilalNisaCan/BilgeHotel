using Microsoft.EntityFrameworkCore;
using Project.Dal.ContextClasses;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Concretes
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {


        public UserRepository(MyContext context) : base(context)
        {
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserWithReservationsAsync(int userId)
        {
            return await _dbSet.Include(u => u.Reservations)
                                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<User>> GetUsersWithRoleAsync(UserRole role)
        {
            return await _dbSet.Where(u => u.Role == role).ToListAsync();
        }

        public async Task<User> GetAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(predicate);
        }

        public async Task<User?> GetByUserNameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.UserName == username);
        }
    }
}
