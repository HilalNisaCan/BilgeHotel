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
        /// Oluşturulan raporları loglar.
        /// </summary>
        public async Task LogReportAsync(ReportLogDto logDto)
        {
            ReportLog entity = _mapper.Map<ReportLog>(logDto);
            await _reportLogRepository.AddAsync(entity);
        }
    }
}
