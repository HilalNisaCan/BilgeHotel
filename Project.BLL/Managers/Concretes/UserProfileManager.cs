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

        public async Task<int> CreateUserProfileAsync(int userId, UserProfileDto profileDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            var userProfile = _mapper.Map<UserProfile>(profileDto);
            userProfile.UserId = userId;

            await _userProfileRepository.AddAsync(userProfile);
            return userProfile.Id;
        }

        public async Task<bool> UpdateUserProfileAsync(int userId, UserProfileDto profileDto)
        {
            var existingProfile = await _userProfileRepository.GetAsync(p => p.UserId == userId);
            if (existingProfile == null)
                return false;

            _mapper.Map(profileDto, existingProfile);
            await _userProfileRepository.UpdateAsync(existingProfile);

            return true;
        }

        public async Task<UserProfileDto> GetUserProfileByUserIdAsync(int userId)
        {
            var profile = await _userProfileRepository.GetAsync(p => p.UserId == userId);
            return profile == null ? null : _mapper.Map<UserProfileDto>(profile);
        }
    }
}
