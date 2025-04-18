using Microsoft.AspNetCore.Mvc;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using System.Text;
using ClosedXML.Excel;
using System.Reflection.Metadata;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using QuestPDF.Previewer;
using Project.MvcUI.Areas.Admin.PdfDocuments;
using Project.BLL.Managers.Concretes;

namespace Project.MvcUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/CustomerReports")]
    public class CustomerReportController : Controller
    {
        private readonly ICustomerManager _customerManager;

        public CustomerReportController(ICustomerManager customerManager)
        {
            _customerManager = customerManager;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            List<CustomerReportDto> reportList = await _customerManager.GetAllCustomerReportsAsync();
            return View(reportList);
        }

        [HttpGet("ExportToExcel")]
        public async Task<IActionResult> ExportToExcel()
        {
            List<CustomerReportDto> reports = await _customerManager.GetAllCustomerReportsAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Müşteri Raporları");

                worksheet.Cell(1, 1).Value = "Ad Soyad";
                worksheet.Cell(1, 2).Value = "TC Kimlik";
                worksheet.Cell(1, 3).Value = "Telefon";
                worksheet.Cell(1, 4).Value = "Rezervasyon";
                worksheet.Cell(1, 5).Value = "Toplam Harcama";
                worksheet.Cell(1, 6).Value = "Sadakat Puanı";
                worksheet.Cell(1, 7).Value = "Kampanya";
                worksheet.Cell(1, 8).Value = "Son Rezervasyon";

                int row = 2;
                foreach (var item in reports)
                {
                    worksheet.Cell(row, 1).Value = item.FullName;
                    worksheet.Cell(row, 2).Value = item.IdentityNumber;
                    worksheet.Cell(row, 3).Value = item.PhoneNumber;
                    worksheet.Cell(row, 4).Value = item.TotalReservationCount;
                    worksheet.Cell(row, 5).Value = item.TotalSpent;
                    worksheet.Cell(row, 6).Value = item.LoyaltyPoints;
                    worksheet.Cell(row, 7).Value = item.CampaignUsageCount;
                    worksheet.Cell(row, 8).Value = item.LastReservationDate?.ToString("dd.MM.yyyy");
                    row++;
                }

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var content = stream.ToArray();

                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MusteriRaporlari.xlsx");
            }
        }

        [HttpGet("ExportToPdf")]
        public async Task<IActionResult> ExportToPdf()
        {
            List<CustomerReportDto> reports = await _customerManager.GetAllCustomerReportsAsync();

            var document = new CustomerReportPdfDocument(reports);
            byte[] pdfBytes = document.GeneratePdf();

            return File(pdfBytes, "application/pdf", $"MusteriRaporlari_{DateTime.Now:yyyyMMdd}.pdf");
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var report = (await _customerManager.GetAllCustomerReportsAsync()).FirstOrDefault(x => x.Id == id);
            if (report == null)
                return NotFound();

            return View(report);
        }
    }


}
