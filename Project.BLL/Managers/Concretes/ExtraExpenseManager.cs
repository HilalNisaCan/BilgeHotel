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
{  /// <summary>
   /// ExtraExpense işlemlerini gerçekleştiren manager sınıfı.
   /// </summary>
    public class ExtraExpenseManager : BaseManager<ExtraExpenseDto, ExtraExpense>, IExtraExpenseManager
    {
        private readonly IExtraExpenseRepository _extraExpenseRepository;
        private readonly IMapper _mapper;

        public ExtraExpenseManager(IExtraExpenseRepository extraExpenseRepository, IMapper mapper)
            : base(extraExpenseRepository, mapper)
        {
            _extraExpenseRepository = extraExpenseRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Yeni masraf ekler.
        /// </summary>
        public async Task<bool> AddExpenseAsync(ExtraExpenseDto dto)
        {
            var entity = _mapper.Map<ExtraExpense>(dto);
            await _extraExpenseRepository.AddAsync(entity);
            return true;
        }

        /// <summary>
        /// Mevcut masrafı günceller.
        /// </summary>
        public async Task<bool> UpdateExpenseAsync(ExtraExpenseDto dto)
        {
            var entity = await _extraExpenseRepository.GetByIdAsync(dto.Id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);
            await _extraExpenseRepository.UpdateAsync(entity);
            return true;
        }

        /// <summary>
        /// Masraf kaydını siler.
        /// </summary>
        public async Task<bool> DeleteExpenseAsync(int id)
        {
            var entity = await _extraExpenseRepository.GetByIdAsync(id);
            if (entity == null) return false;

            await _extraExpenseRepository.RemoveAsync(entity);
            return true;
        }

        /// <summary>
        /// Belirli bir rezervasyona ait tüm masrafları getirir.
        /// </summary>
        public async Task<List<ExtraExpenseDto>> GetExpensesByReservationAsync(int reservationId)
        {
            List<ExtraExpense> list = (List<ExtraExpense>)await _extraExpenseRepository
           .GetAllAsync(x => x.ReservationId == reservationId);

            return _mapper.Map<List<ExtraExpenseDto>>(list);
        }

        /// <summary>
        /// Belirli bir masrafı getirir.
        /// </summary>
        public async Task<ExtraExpenseDto> GetExpenseByIdAsync(int id)
        {
            var entity = await _extraExpenseRepository.GetByIdAsync(id);
            return _mapper.Map<ExtraExpenseDto>(entity);
        }

        /// <summary>
        /// Masrafın durumunu günceller.
        /// </summary>
        public async Task<bool> UpdateExpenseStatusAsync(int expenseId, string status)
        {
            var entity = await _extraExpenseRepository.GetByIdAsync(expenseId);
            if (entity == null) return false;

            if (!Enum.TryParse(status, out DataStatus parsedStatus))
                return false;

            entity.Status = parsedStatus;
            await _extraExpenseRepository.UpdateAsync(entity);
            return true;
        }

        public async Task AddAsync(ExtraExpenseDto dto)
        {
            ExtraExpense entity = _mapper.Map<ExtraExpense>(dto);
            await _extraExpenseRepository.AddAsync(entity);
        }
    }
}
