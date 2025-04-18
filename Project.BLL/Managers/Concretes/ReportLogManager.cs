using AutoMapper;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Dal.Repositories.Abstracts;
using Project.Dal.Repositories.Concretes;
using Project.Entities.Enums;
using Project.Entities.Interfaces;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Project.BLL.Managers.Concretes
{
    public class ReportLogManager : BaseManager<ReportLogDto, ReportLog>, IReportLogManager
    {
        private readonly IReportLogRepository _reportLogRepository;
        private readonly IMapper _mapper;

        public ReportLogManager(IReportLogRepository reportLogRepository, IMapper mapper)
            : base(reportLogRepository, mapper)
        {
            _reportLogRepository = reportLogRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Oluşturulan raporu veritabanına kaydeder. Başarılıysa true döner.
        /// </summary>
        public async Task<bool> CreateReportLogAsync(ReportLogDto dto)
        {
            // DTO → Entity dönüşümü
            ReportLog entity = _mapper.Map<ReportLog>(dto);

            // Veritabanına kaydet
            await _reportLogRepository.AddLogAsync(entity);
            return true;
        }
    }
}
