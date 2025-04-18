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
    /// Misafir ziyaret kayıtlarını yöneten manager sınıfı.
    /// </summary>
    public class GuestVisitLogManager : BaseManager<GuestVisitLogDto, GuestVisitLog>, IGuestVisitLogManager
    {
        private readonly IGuestVisitLogRepository _visitLogRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GuestVisitLogManager(IGuestVisitLogRepository visitLogRepository, IRoomRepository roomRepository, ICustomerRepository customerRepository, IMapper mapper)
            : base(visitLogRepository, mapper)
        {
            _visitLogRepository = visitLogRepository;
            _roomRepository = roomRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Müşteriyi odaya giriş olarak kaydeder. Oda müsait olmalı, son çıkıştan 24 saat geçmeli.
        /// </summary>
        public async Task<bool> RegisterGuestEntryAsync(int customerId, int roomId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            var room = await _roomRepository.GetByIdAsync(roomId);

            if (customer == null || room == null || room.RoomStatus != RoomStatus.Available)
                return false;

            // Aynı müşteri zaten içeride mi?
            var existingVisit = await _visitLogRepository.GetAllAsync(v => v.CustomerId == customerId && v.ExitDate == null);
            if (existingVisit.Any())
                throw new Exception("Bu müşteri zaten giriş yapmış.");

            // Son çıkıştan 24 saat geçmesi kontrolü
            var lastVisit = (await _visitLogRepository.GetAllAsync(v => v.CustomerId == customerId && v.ExitDate != null))
                            .OrderByDescending(v => v.ExitDate)
                            .FirstOrDefault();

            if (lastVisit != null && (DateTime.UtcNow - lastVisit.ExitDate.Value).TotalHours < 24)
                throw new Exception("Bu müşteri son çıkışından sonra 24 saat geçmeden tekrar giriş yapamaz.");

            var visitLog = new GuestVisitLog
            {
                CustomerId = customerId,
                RoomId = roomId,
                EntryDate = DateTime.UtcNow
            };

            await _visitLogRepository.AddAsync(visitLog);

            // Oda durumunu "Dolu" olarak güncelle
            room.RoomStatus = RoomStatus.Occupied;
            await _roomRepository.UpdateAsync(room);

            return true;
        }

        /// <summary>
        /// Müşteri çıkış kaydını oluşturur. Oda durumu "Temizlikte" olarak güncellenir.
        /// </summary>
        public async Task<bool> RegisterGuestExitAsync(int customerId)
        {
            var visitLog = (await _visitLogRepository.GetAllAsync(v => v.CustomerId == customerId && v.ExitDate == null)).FirstOrDefault();
            if (visitLog == null)
                throw new Exception("Bu müşteri için aktif bir giriş kaydı bulunamadı.");

            visitLog.ExitDate = DateTime.UtcNow;
            await _visitLogRepository.UpdateAsync(visitLog);

            // Oda durumu "Temizlikte" olarak güncellenir
            var room = await _roomRepository.GetByIdAsync(visitLog.RoomId);
            room.RoomStatus = RoomStatus.Cleaning;
            await _roomRepository.UpdateAsync(room);

            return true;
        }

        /// <summary>
        /// Müşterinin tüm ziyaret geçmişini getirir.
        /// </summary>
        public async Task<List<GuestVisitLogDto>> GetGuestVisitHistoryAsync(int customerId)
        {
            var logs = await _visitLogRepository.GetAllAsync(v => v.CustomerId == customerId);
            return _mapper.Map<List<GuestVisitLogDto>>(logs.OrderByDescending(v => v.EntryDate).ToList());
        }

        public async Task AddAsync(GuestVisitLogDto dto)
        {
            GuestVisitLog entity = _mapper.Map<GuestVisitLog>(dto);
            await _visitLogRepository.AddAsync(entity);
        }
    }
}
