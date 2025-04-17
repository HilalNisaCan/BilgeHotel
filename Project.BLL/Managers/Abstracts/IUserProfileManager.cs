using Project.BLL.DtoClasses;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Abstracts
{
    public interface IUserProfileManager : IManager<UserProfileDto, UserProfile>
    {
        /// <summary>
        /// Verilen kullanıcı için yeni bir profil oluşturur.
        /// </summary>
        Task<int> CreateUserProfileAsync(int userId, UserProfileDto profileDto); // ✅ Kullanıcı profili oluştur

        /// <summary>
        /// Kullanıcının mevcut profilini günceller.
        /// </summary>
        Task<bool> UpdateUserProfileAsync(int userId, UserProfileDto profileDto); // ✅ Kullanıcı profilini güncelle

        /// <summary>
        /// Belirtilen kullanıcı ID’sine ait profil bilgilerini döner.
        /// </summary>
        Task<UserProfileDto> GetUserProfileByUserIdAsync(int userId); // ✅ Kullanıcı profili getir
    }
}
