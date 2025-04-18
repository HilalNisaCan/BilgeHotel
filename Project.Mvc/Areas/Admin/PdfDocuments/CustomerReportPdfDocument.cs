using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Project.BLL.DtoClasses;

namespace Project.MvcUI.Areas.Admin.PdfDocuments
{

    public class CustomerReportPdfDocument : IDocument
    {
        private readonly List<CustomerReportDto> _reports;

        public CustomerReportPdfDocument(List<CustomerReportDto> reports)
        {
            _reports = reports;
        }

        public DocumentMetadata GetMetadata()
        {
            return new DocumentMetadata
            {
                Title = "Müşteri Raporları",
                Author = "BilgeHotel",
                Subject = "Müşteri Rezervasyon ve Harcama Bilgileri",
                Keywords = "Müşteri,Rapor,BilgeHotel"
            };
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header().AlignCenter().Text("Müşteri Raporları")
                    .SemiBold().FontSize(18).FontColor(Colors.Blue.Medium);

                page.Content().Element(ComposeContent);

                page.Footer().AlignCenter().Text(text =>
                {
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                });
            });
        }

        void ComposeContent(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2); // Ad Soyad
                    columns.RelativeColumn(2); // TC
                    columns.RelativeColumn(2); // Telefon
                    columns.RelativeColumn(2); // Rezervasyon
                    columns.RelativeColumn(2); // Harcama
                    columns.RelativeColumn(2); // Sadakat
                    columns.RelativeColumn(2); // Kampanya
                    columns.RelativeColumn(2); // Son Konaklama
                });

                table.Header(header =>
                {
                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Ad Soyad").Bold();
                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("TC Kimlik").Bold();
                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Telefon").Bold();
                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Rezervasyon").Bold();
                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Harcama").Bold();
                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Sadakat").Bold();
                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Kampanya").Bold();
                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Son Konaklama").Bold();
                });

                foreach (var report in _reports)
                {
                    table.Cell().Padding(5).Text(report.FullName);
                    table.Cell().Padding(5).Text(report.IdentityNumber);
                    table.Cell().Padding(5).Text(report.PhoneNumber);
                    table.Cell().Padding(5).Text(report.TotalReservationCount.ToString());
                    table.Cell().Padding(5).Text(report.TotalSpent.ToString("C"));
                    table.Cell().Padding(5).Text(report.LoyaltyPoints.ToString());
                    table.Cell().Padding(5).Text(report.CampaignUsageCount.ToString());
                    table.Cell().Padding(5).Text(report.LastReservationDate?.ToString("dd.MM.yyyy") ?? "-");
                }
            });
        }

        public byte[] GeneratePdf()
        {
            return QuestPDF.Fluent.Document.Create(Compose).GeneratePdf();
        }
    }
}