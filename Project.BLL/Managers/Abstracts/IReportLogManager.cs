using Project.BLL.DtoClasses;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Abstracts
{
    public interface IReportLogManager : IManager<ReportLogDto, ReportLog>
    {
        /// <summary>
        /// Oluşturulan raporları veritabanına kaydeder (XML, Finansal vb.).
        /// </summary>
        Task<bool> CreateReportLogAsync(ReportLogDto dto);
    }
}

