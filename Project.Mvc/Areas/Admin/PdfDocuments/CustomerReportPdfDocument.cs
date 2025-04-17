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

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        [Obsolete]
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Background(Colors.White);

                page.Header()
                    .AlignCenter()
                    .Text("Müşteri Raporları")
                    .FontSize(18)
                    .SemiBold()
                    .FontColor(Colors.Blue.Medium);

                page.Content().PaddingVertical(10).Column(column =>
                {
                    column.Spacing(10);

                    // Tablo başlığı
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(30);   // #
                            columns.RelativeColumn(2);    // Ad Soyad
                            columns.RelativeColumn(2);    // TC
                            columns.RelativeColumn(2);    // Telefon
                            columns.ConstantColumn(40);   // Rez.
                            columns.RelativeColumn(2);    // Harcama
                            columns.ConstantColumn(40);   // Puan
                            columns.ConstantColumn(40);   // Kamp.
                            columns.RelativeColumn(2);    // Son Tarih
                        });

                        // Başlıklar
                        string[] headers = { "#", "Ad Soyad", "TC", "Telefon", "Rez.", "Harcama", "Puan", "Kamp.", "Son Tarih" };
                        foreach (var title in headers)
                        {
                            table.Cell().Element(CellStyle).Text(title).Bold();
                        }

                        int index = 1;
                        foreach (var item in _reports)
                        {
                            table.Cell().Element(CellStyle).Text(index++.ToString());
                            table.Cell().Element(CellStyle).Text(item.FullName);
                            table.Cell().Element(CellStyle).Text(item.IdentityNumber);
                            table.Cell().Element(CellStyle).Text(item.PhoneNumber);
                            table.Cell().Element(CellStyle).Text(item.ReservationCount.ToString());
                            table.Cell().Element(CellStyle).Text(item.TotalSpent.ToString("C2"));
                            table.Cell().Element(CellStyle).Text(item.LoyaltyPoints.ToString());
                            table.Cell().Element(CellStyle).Text(item.CampaignCount.ToString());
                            table.Cell().Element(CellStyle).Text(item.LastReservationDate.HasValue
                            ? item.LastReservationDate.Value.ToString("dd.MM.yyyy")
                            : "Yok");
                        }
                    });
                });

                page.Footer().PaddingTop(15).Column(column =>
                {
                    column.Spacing(5);

                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Text("Hazırlayan: Hilal Nisa Canpolat").Italic().FontSize(10);
                        row.RelativeItem().AlignRight().Text($"Tarih: {DateTime.Now:dd.MM.yyyy}").FontSize(10);
                    });

                    column.Item().AlignCenter().Text("Bu belge BilgeHotel Yönetim Paneli tarafından oluşturulmuştur.")
                          .FontSize(9).Italic().FontColor(Colors.Grey.Darken2);
                });
            });
        }

        private static IContainer CellStyle(IContainer container)
        {
            return container.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5);
        }
    }
}
