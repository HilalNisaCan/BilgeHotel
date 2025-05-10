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
        private readonly IMapper _mapper;

        public GuestVisitLogManager(
            IGuestVisitLogRepository visitLogRepository,
            IMapper mapper)
            : base(visitLogRepository, mapper)
        {
            _visitLogRepository = visitLogRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Yeni bir ziyaret kaydı ekler.
        /// </summary>
        public async Task AddAsync(GuestVisitLogDto dto)
        {
            GuestVisitLog entity = _mapper.Map<GuestVisitLog>(dto);
            await _visitLogRepository.AddAsync(entity);
        }
    }
}