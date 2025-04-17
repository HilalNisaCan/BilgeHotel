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
    public class UserProfileRepository : BaseRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(MyContext context) : base(context)
        {
        }

        public async Task<UserProfile> GetAsync(Expression<Func<UserProfile, bool>> predicate)
        {
            return await _context.Set<UserProfile>().FirstOrDefaultAsync(predicate);
        }

        public async Task<UserProfile> GetByUserIdAsync(int userId)
        {
            return await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<UserProfile> GetProfileByUserIdAsync(int userId)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.UserId == userId);
        }
    }
}
