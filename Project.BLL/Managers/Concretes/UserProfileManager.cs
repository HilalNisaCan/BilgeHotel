using AutoMapper;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Concretes
{
    public class UserProfileManager : BaseManager<UserProfileDto, UserProfile>, IUserProfileManager
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserProfileManager(IUserProfileRepository userProfileRepository, IUserRepository userRepository, IMapper mapper)
            : base(userProfileRepository, mapper)
        {
            _userProfileRepository = userProfileRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Kullanıcıya ait yeni bir profil oluşturur.
        /// Kullanıldığı yer: Kullanıcı kaydı tamamlandıktan sonra profil detayları girildiğinde.
        /// </summary>
        /// <param name="userId">Profil atanacak kullanıcı ID'si</param>
        /// <param name="profileDto">Oluşturulacak profilin DTO verisi</param>
        /// <returns>Oluşturulan profilin veritabanı ID'si</returns>
        public async Task<int> CreateUserProfileAsync(int userId, UserProfileDto profileDto)
        {
            User? user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            UserProfile userProfile = _mapper.Map<UserProfile>(profileDto);
            userProfile.UserId = userId;

            await _userProfileRepository.AddAsync(userProfile);
            return userProfile.Id;
        }

        /// <summary>
        /// Mevcut kullanıcı profiline güncelleme yapar.
        /// Kullanıldığı yer: Kullanıcı ayarlarında profil bilgileri değiştirildiğinde.
        /// </summary>
        /// <param name="userId">Güncellenecek kullanıcının ID'si</param>
        /// <param name="profileDto">Yeni profil bilgileri</param>
        /// <returns>Güncelleme başarılıysa true, değilse false</returns>
        public async Task<bool> UpdateUserProfileAsync(int userId, UserProfileDto profileDto)
        {
            UserProfile? existingProfile = await _userProfileRepository.GetAsync(p => p.UserId == userId);
            if (existingProfile == null)
                return false;

            _mapper.Map(profileDto, existingProfile);
            await _userProfileRepository.UpdateAsync(existingProfile);

            return true;
        }
        /// <summary>
        /// Kullanıcı ID'sine göre ilgili kullanıcının profilini getirir.
        /// Kullanıldığı yer: Kullanıcı detay ekranı veya profil sayfası görüntülemede.
        /// </summary>
        /// <param name="userId">Kullanıcının ID'si</param>
        /// <returns>Kullanıcının profil DTO'su, yoksa null</returns>
        public async Task<UserProfileDto> GetUserProfileByUserIdAsync(int userId)
        {
            UserProfile? profile = await _userProfileRepository.GetAsync(p => p.UserId == userId);
            return profile == null ? null : _mapper.Map<UserProfileDto>(profile);
        }
    }
}
