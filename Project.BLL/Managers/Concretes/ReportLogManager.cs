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
        private readonly IReportLogRepository _reportRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public ReportLogManager(IReportLogRepository reportRepository,
                                IReservationRepository reservationRepository,
                                IPaymentRepository paymentRepository,
                                IMapper mapper)
            : base(reportRepository, mapper)
        {
            _reportRepository = reportRepository;
            _reservationRepository = reservationRepository;
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Günlük müşteri girişlerini raporlar.
        /// </summary>
        public async Task GenerateDailyCustomerReportAsync()
        {
            var reservations = await _reservationRepository.GetAllAsync(r => r.StartDate.Date == DateTime.UtcNow.Date);

            string reportData = $"Bugün otele giriş yapan müşteri sayısı: {reservations.Count()}";

            var report = new ReportLog
            {
                ReportType = ReportType.DailyGuestReport,
                CreatedDate = DateTime.UtcNow,
                ReportStatus = ReportStatus.Success,
                ReportData = reportData
            };

            await _reportRepository.AddAsync(report);
        }


        /// <summary>
        /// Günlük ödeme ve iptal verilerinden mali rapor oluşturur.
        /// </summary>
        public async Task GenerateFinancialReportAsync()
        {
            var payments = await _paymentRepository.GetAllAsync(p => p.PaymentDate.Date == DateTime.UtcNow.Date);
            decimal totalRevenue = payments.Sum(p => p.TotalAmount);

            var expenses = await _paymentRepository.GetAllAsync(p => p.PaymentDate.Date == DateTime.UtcNow.Date && p.PaymentStatus == PaymentStatus.Cancelled);
            decimal totalExpenses = expenses.Sum(p => p.TotalAmount);

            var reservations = await _reservationRepository.GetAllAsync(r => r.StartDate.Date == DateTime.UtcNow.Date);

            var report = new ReportLog
            {
                ReportType = ReportType.FinancialReport,
                CreatedDate = DateTime.UtcNow,
                ReportStatus = ReportStatus.Success,
                ReportData = $"Bugünkü toplam gelir: {totalRevenue:C}, Toplam giderler: {totalExpenses:C}, Toplam rezervasyon: {reservations.Count()}"
            };

            await _reportRepository.AddAsync(report);
        }

        /// <summary>
        /// Belirtilen rapor türüne göre XML formatında çıktı oluşturur.
        /// </summary>
        public async Task<string> GenerateXmlReportAsync(ReportType reportType)
        {
            var reports = await _reportRepository.GetAllAsync(r => r.ReportType == reportType);
            XDocument xmlDocument = new XDocument(
                new XElement("Reports",
                    reports.Select(report =>
                        new XElement("Report",
                            new XElement("Id", report.Id),
                            new XElement("CreatedDate", report.CreatedDate),
                            new XElement("Status", report.Status.ToString()),
                            new XElement("Data", report.ReportData)
                        )
                    )
                )
            );

            return xmlDocument.ToString();
        }

        /// <summary>
        /// Belirli bir rapor türüne ait kayıtları DTO formatında döner.
        /// </summary>
        public async Task<List<ReportLogDto>> GetReportsAsync(ReportType reportType)
        {
            var reports = await _reportRepository.GetAllAsync(r => r.ReportType == reportType);
            return _mapper.Map<List<ReportLogDto>>(reports);
        }

        /// <summary>
        /// Oluşturulan XML raporunu belirtilen sunucuya gönderir.
        /// </summary>
        public async Task<bool> SendXmlReportToAuthoritiesAsync(string xmlData, string endpointUrl)
        {
            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(xmlData, Encoding.UTF8, "application/xml");
                HttpResponseMessage response = await httpClient.PostAsync(endpointUrl, content);
                return response.IsSuccessStatusCode;
            }
        }
    }
}
