using Project.BLL.DtoClasses;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Abstracts
{
    /// <summary>
    /// Sistem kullanıcılarıyla ilgili işlemleri tanımlar.
    /// </summary>
    public interface IUserManager : IManager<UserDto, User>
    {
        /// <summary>
        /// Yeni kullanıcı oluşturur (şifre hash'lenir).
        /// </summary>
        Task<UserDto> RegisterAsync(UserDto userDto);

        /// <summary>
        /// Kullanıcı girişi yapar (şifre doğrulaması içerir).
        /// </summary>
        Task<UserDto> LoginAsync(string email, string password);

        /// <summary>
        /// Kullanıcının mevcut rolünü günceller.
        /// </summary>
        Task<bool> ChangeUserRoleAsync(int userId, UserRole newRole);

        /// <summary>
        /// Kullanıcının aktiflik durumunu günceller.
        /// </summary>
        Task<bool> SetActivationStatusAsync(int userId, bool isActive);

        /// <summary>
        /// Tüm aktif kullanıcıları listeler.
        /// </summary>
        Task<List<UserDto>> GetActiveUsersAsync();

        /// <summary>
        /// Kullanıcı adından kullanıcıyı getirir.
        /// </summary>
        Task<UserDto?> GetByUserNameAsync(string username);
    }
}
