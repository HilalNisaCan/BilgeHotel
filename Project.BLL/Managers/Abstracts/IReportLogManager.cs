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
        Task LogReportAsync(ReportLogDto logDto);
    }
}

