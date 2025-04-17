using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Enums
{
    public enum ReportType
    {
        DailyGuestReport = 0,       // Valiliğe gönderilen günlük müşteri raporu
        FinancialReport = 1,        // Gelir-gider tabloları
        ReservationReport = 2,      // Yapılan rezervasyonların raporu
        OccupancyRateReport = 3     // Otel doluluk oranı analizi
    }
}
