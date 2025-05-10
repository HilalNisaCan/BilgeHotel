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
        /// Belirli bir rezervasyona ait tüm masrafları getirir.
        /// </summary>
        public async Task<List<ExtraExpenseDto>> GetExpensesByReservationAsync(int reservationId)
        {
            List<ExtraExpense> list = (List<ExtraExpense>)await _extraExpenseRepository
                .GetAllAsync(x => x.ReservationId == reservationId);

            return _mapper.Map<List<ExtraExpenseDto>>(list);
        }

        /// <summary>
        /// Yeni masraf kaydı ekler.
        /// </summary>
        public async Task AddAsync(ExtraExpenseDto dto)
        {
            ExtraExpense entity = _mapper.Map<ExtraExpense>(dto);
            await _extraExpenseRepository.AddAsync(entity);
        }
    }
}
