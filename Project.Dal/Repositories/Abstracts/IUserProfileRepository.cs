using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IUserProfileRepository:IRepository<UserProfile>
    {
        Task<UserProfile> GetProfileByUserIdAsync(int userId); // Kullanıcının profil bilgilerini getir
        Task<UserProfile> GetAsync(Expression<Func<UserProfile, bool>> predicate); // ✅ Kullanıcı profili sorgulamak için
        Task<UserProfile> GetByUserIdAsync(int userId);

    }
}
