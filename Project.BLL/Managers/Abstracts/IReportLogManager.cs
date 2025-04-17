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
        /// Günlük müşteri girişlerini raporlar.
        /// </summary>
        Task GenerateDailyCustomerReportAsync();

        /// <summary>
        /// Günlük ödeme ve iptal verilerinden mali rapor oluşturur.
        /// </summary>
        Task GenerateFinancialReportAsync();

        /// <summary>
        /// Belirli bir rapor türüne göre XML formatında çıktı oluşturur.
        /// </summary>
        Task<string> GenerateXmlReportAsync(ReportType reportType);

        /// <summary>
        /// Rapor türüne göre ilgili logları DTO olarak getirir.
        /// </summary>
        Task<List<ReportLogDto>> GetReportsAsync(ReportType reportType);

        /// <summary>
        /// XML çıktısını dış kuruma gönderir.
        /// </summary>
        Task<bool> SendXmlReportToAuthoritiesAsync(string xmlData, string endpointUrl);
    }
}

