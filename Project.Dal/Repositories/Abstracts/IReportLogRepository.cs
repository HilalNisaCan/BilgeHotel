using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IReportLogRepository:IRepository<ReportLog>
    {
       

        // XML veya sistem loglarını özel olarak eklemek için kullanılacak
        Task AddLogAsync(ReportLog log);
    }
}

