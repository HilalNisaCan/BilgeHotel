using AutoMapper;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
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
    /// Şikayet kayıtlarını yöneten manager sınıfı.
    /// </summary>
    public class ComplaintLogManager : BaseManager<ComplaintLogDto, ComplaintLog>, IComplaintLogManager
    {
        private readonly IComplaintLogRepository _complaintRepository;
        private readonly IMapper _mapper;

        public ComplaintLogManager(IComplaintLogRepository complaintRepository, IMapper mapper)
            : base(complaintRepository, mapper)
        {
            _complaintRepository = complaintRepository;
            _mapper = mapper;
        }



        /// <summary>
        /// Şikayete verilen cevabı kaydeder.
        /// </summary>
        public async Task<bool> RespondToComplaintAsync(int complaintId, string response)
        {
            ComplaintLog? entity = await _complaintRepository.GetByIdAsync(complaintId);
            if (entity == null) return false;

            entity.Response = response;
            entity.ComplaintStatus = ComplaintStatus.Responded;
            entity.IsResolved = true;

            await _complaintRepository.UpdateAsync(entity);
            return true;
        }

        /// <summary>
        /// Şikayet kaydını siler.
        /// </summary>
        public async Task<bool> DeleteComplaintAsync(int complaintId)
        {
            ComplaintLog? entity = await _complaintRepository.GetByIdAsync(complaintId);
            if (entity == null) return false;

            await _complaintRepository.RemoveAsync(entity);
            return true;
        }
    }
}
