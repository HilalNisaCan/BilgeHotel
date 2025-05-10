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
        private readonly IAppRoleRepository _appRoleRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserManager(IUserRepository userRepository, IMapper mapper, IPasswordHasher<User> passwordHasher, IAppRoleRepository appRoleRepository)
            : base(userRepository, mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _appRoleRepository = appRoleRepository;
        }

        /// <summary>
        /// Yeni kullanıcı oluşturur ve şifresini hash'leyerek kaydeder.
        /// Kullanıldığı yer: Kullanıcı kayıt (register) işlemi sırasında.
        /// </summary>
        public async Task<UserDto> RegisterAsync(UserDto userDto)
        {
            bool existing = await _userRepository.AnyAsync(x => x.Email == userDto.Email);
            if (existing)
                throw new Exception("Bu e-posta adresi zaten kullanılmakta.");

            User user = _mapper.Map<User>(userDto);
            user.PasswordHash = _passwordHasher.HashPassword(user, userDto.Password);
            user.IsActivated = true;
            user.CreatedDate = DateTime.Now;
            user.Status = DataStatus.Inserted;

            // Kullanıcıya Customer rolü atanır
            AppRole? role = await _appRoleRepository.GetFirstOrDefaultAsync(
                predicate: r => r.Name == "Customer",
                include: null
            );

            if (role == null)
                throw new Exception("❌ 'Customer' rolü bulunamadı. Lütfen AppRole seed işlemini kontrol edin.");

            user.AppRoleId = role.Id;

            await _userRepository.AddAsync(user);
            return _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// Kullanıcının giriş bilgilerini doğrular (email ve şifre kontrolü).
        /// Kullanıldığı yer: Login ekranı.
        /// </summary>
        public async Task<UserDto> LoginAsync(string email, string password)
        {
            List<User> users = (await _userRepository.GetAllAsync(x => x.Email == email)).ToList();
            User? user = users.FirstOrDefault();

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result != PasswordVerificationResult.Success)
                throw new Exception("Şifre hatalı.");

            if (!user.IsActivated)
                throw new Exception("Kullanıcı hesabı aktif değil.");

            return _mapper.Map<UserDto>(user);
        }
        /// <summary>
        /// Kullanıcının sistemdeki rolünü günceller.
        /// Kullanıldığı yer: Admin panelde kullanıcı rolü değiştirme.
        /// </summary>
        public async Task<bool> ChangeUserRoleAsync(int userId, string newRoleName)
        {
            User? user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            AppRole? newRole = await _appRoleRepository.GetByNameAsync(newRoleName);
            if (newRole == null)
                return false;

            user.AppRoleId = newRole.Id;
            await _userRepository.UpdateAsync(user);

            return true;
        }


        /// <summary>
        /// Kullanıcının aktiflik durumunu günceller (aktif/pasif).
        /// Kullanıldığı yer: Admin panelde kullanıcı devre dışı bırakma veya aktif etme.
        /// </summary>
        public async Task<bool> SetActivationStatusAsync(int userId, bool isActive)
        {
            User? user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            user.IsActivated = isActive;
            await _userRepository.UpdateAsync(user);

            return true;
        }

        /// <summary>
        /// Sistemde aktif durumda olan tüm kullanıcıları getirir.
        /// Kullanıldığı yer: Admin panelde kullanıcı listesi.
        /// </summary>
        public async Task<List<UserDto>> GetActiveUsersAsync()
        {
            List<User> users = (await _userRepository.GetAllAsync(x => x.IsActivated)).ToList();
            return _mapper.Map<List<UserDto>>(users);
        }


        /// <summary>
        /// Kullanıcı adı veya email'e göre kullanıcıyı getirir.
        /// Kullanıldığı yer: Login, yorum, işlem kayıtlarında kullanıcı doğrulaması için.
        /// </summary>
        public async Task<UserDto?> GetByUserNameAsync(string username)
        {
            User? user = await _userRepository.GetByUserNameAsync(username);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }
    }
}
