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
        /// Yeni bir şikayet kaydı oluşturur.
        /// </summary>
        public async Task<bool> AddComplaintAsync(int customerId, string subject, string description)
        {
            var entity = new ComplaintLog
            {
                CustomerId = customerId,
                Description = description,
                ComplaintStatus = ComplaintStatus.Pending,
                CreatedDate = DateTime.UtcNow
            };

            await _complaintRepository.AddAsync(entity);
            return true;
        }

        /// <summary>
        /// Tüm şikayet kayıtlarını getirir.
        /// </summary>
        public async Task<List<ComplaintLogDto>> GetComplaintsAsync()
        {
            var list = await _complaintRepository.GetAllAsync();
            return _mapper.Map<List<ComplaintLogDto>>(list);
        }

        /// <summary>
        /// Belirli müşteriye ait şikayet kayıtlarını getirir.
        /// </summary>
        public async Task<List<ComplaintLogDto>> GetCustomerComplaintsAsync(int customerId)
        {
            var list = await _complaintRepository.GetAllAsync(x => x.CustomerId == customerId);
            return _mapper.Map<List<ComplaintLogDto>>(list);
        }

        /// <summary>
        /// Şikayete verilen cevabı kaydeder.
        /// </summary>
        public async Task<bool> RespondToComplaintAsync(int complaintId, string response)
        {
            var entity = await _complaintRepository.GetByIdAsync(complaintId);
            if (entity == null) return false;

            // Yalnızca yanıtla ilgili alanlar güncelleniyor
            entity.Response = response;
            entity.ComplaintStatus = ComplaintStatus.Responded;
            entity.IsResolved = true;

            await _complaintRepository.UpdateAsync(entity);
            return true;
        }

        /// <summary>
        /// Şikayet durumunu günceller.
        /// </summary>
        public async Task<bool> UpdateComplaintStatusAsync(int complaintId, ComplaintStatus status)
        {
            var entity = await _complaintRepository.GetByIdAsync(complaintId);
            if (entity == null) return false;

            entity.ComplaintStatus = status;
            await _complaintRepository.UpdateAsync(entity);
            return true;
        }

        /// <summary>
        /// Şikayet kaydını sistemden siler.
        /// </summary>
        public async Task<bool> DeleteComplaintAsync(int complaintId)
        {
            var entity = await _complaintRepository.GetByIdAsync(complaintId);
            if (entity == null) return false;

            await _complaintRepository.RemoveAsync(entity);
            return true;
        }
    }
}
