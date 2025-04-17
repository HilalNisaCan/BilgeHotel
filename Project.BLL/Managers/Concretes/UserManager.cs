using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.BLL.Services.abstracts;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Concretes
{

    /// <summary>
    /// Kullanıcı işlemlerini yöneten manager sınıfı.
    /// </summary>
    public class UserManager : BaseManager<UserDto, User>, IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserManager(IUserRepository userRepository, IMapper mapper, IPasswordHasher<User> passwordHasher)
            : base(userRepository, mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Yeni kullanıcı oluşturur (şifre hash'lenerek kaydedilir).
        /// </summary>
        public async Task<UserDto> RegisterAsync(UserDto userDto)
        {
            var existing = await _userRepository.AnyAsync(x => x.Email == userDto.Email);
            if (existing)
                throw new Exception("Bu e-posta adresi zaten kullanılmakta.");

            var user = _mapper.Map<User>(userDto);
            user.PasswordHash = _passwordHasher.HashPassword(user, userDto.Password);
            user.IsActivated = true;
            user.Role = UserRole.Customer;

            await _userRepository.AddAsync(user);
            return _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// Giriş yapan kullanıcıyı doğrular (şifre eşleşmeli ve kullanıcı aktif olmalı).
        /// </summary>
        public async Task<UserDto> LoginAsync(string email, string password)
        {
            var user = (await _userRepository.GetAllAsync(x => x.Email == email)).FirstOrDefault();
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result != PasswordVerificationResult.Success)
                throw new Exception("Şifre hatalı.");

            if (!user.IsActivated)
                throw new Exception("Kullanıcı hesabı aktif değil.");

            return _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// Kullanıcının sistemdeki rolünü günceller.
        /// </summary>
        public async Task<bool> ChangeUserRoleAsync(int userId, UserRole newRole)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.Role = newRole;
            await _userRepository.UpdateAsync(user);
            return true;
        }

        /// <summary>
        /// Kullanıcının aktiflik durumunu günceller (aktif/pasif).
        /// </summary>
        public async Task<bool> SetActivationStatusAsync(int userId, bool isActive)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.IsActivated = isActive;
            await _userRepository.UpdateAsync(user);
            return true;
        }

        /// <summary>
        /// Aktif tüm kullanıcıları listeler.
        /// </summary>
        public async Task<List<UserDto>> GetActiveUsersAsync()
        {
            var users = await _userRepository.GetAllAsync(x => x.IsActivated);
            return _mapper.Map<List<UserDto>>(users);
        }

       
    }
}
