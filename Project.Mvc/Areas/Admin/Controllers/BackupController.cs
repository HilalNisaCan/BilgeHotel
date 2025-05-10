using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Project.MvcUI.Areas.Admin.Controllers
{
    /*“BackupController, sistemin demo ortamında çalışan yedekleme ve geri yükleme işlemlerini yöneten özel bir controller’dır.
Bu yapı sayesinde, yönetici paneli üzerinden sahte bir yedek dosyası oluşturulabilir ve istenirse sistem dışından alınan bir .bak veya .txt dosyası yüklenebilir.
İşlem sonunda dosya fiziksel olarak wwwroot/DatabaseBackups klasörüne kaydedilir.
Her iki işlem de kullanıcıya TempData üzerinden başarı veya hata mesajı döner.
Geliştirme sürecinde gerçek veritabanı etkileşimi yerine demo amaçlı dosya operasyonları tercih edilmiştir.
Ayrıca tüm dosya işlemleri try-catch bloklarıyla sarılarak hata yönetimi sağlanmıştır.”

  Gerçek veri tabanı işlemleri burada yapılmaz. Sahte (mock) dosya sistemi kullanılır.

RestoreDatabase() → sadece yükleme yapar, sistemde değişiklik yaratmaz.

BackupDatabase() → basit bir .txt dosyası üretir. Gerçek .bak gibi davranmaz ama mantığı gösterir.

Geri yükleme işlemi yapılmasa da proje kapsamı gereği UI ile entegrasyonu sağlanmıştır.
   
     
     */

    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class BackupController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public BackupController(IWebHostEnvironment env)
        {
            _env = env;
        }


        /// <summary>
        /// Tüm mevcut yedek dosyalarını listeler ve view'a gönderir.
        /// </summary>
        public IActionResult Index()
        {
            string backupFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "DatabaseBackups");

            List<string> backupFileNames = new();

            if (Directory.Exists(backupFolder))
            {
                backupFileNames = Directory.GetFiles(backupFolder)
                    .Select(f => Path.GetFileName(f))
                    .OrderByDescending(f => f)
                    .ToList();
            }

            ViewBag.BackupFiles = backupFileNames;

            return View();
        }



        /// <summary>
        /// Veritabanı yedeği yükler (sahte geri yükleme mantığı).
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> RestoreDatabase(IFormFile backupFile)
        {
            if (backupFile == null || backupFile.Length == 0)
            {
                TempData["Error"] = "Lütfen geçerli bir dosya seçin.";
                return RedirectToAction("Index");
            }

            try
            {
                string backupFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "DatabaseBackups");

                if (!Directory.Exists(backupFolder))
                    Directory.CreateDirectory(backupFolder);

                string filePath = Path.Combine(backupFolder, Path.GetFileName(backupFile.FileName));

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await backupFile.CopyToAsync(stream);
                }

                TempData["Success"] = $"📥 '{backupFile.FileName}' dosyası başarıyla yüklendi (gerçek geri yükleme yapılmadı).";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "❌ Dosya yüklenirken hata oluştu: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Veritabanının sahte bir yedeğini oluşturur (demo amaçlı).
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> BackupDatabase()
        {
            try
            {
                string backupFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "DatabaseBackups");

                if (!Directory.Exists(backupFolder))
                    Directory.CreateDirectory(backupFolder);

                string fileName = $"Demo_BilgeHotelBackup_{DateTime.UtcNow:yyyyMMdd_HHmmss}.txt";
                string filePath = Path.Combine(backupFolder, fileName);

                await System.IO.File.WriteAllTextAsync(filePath, $"Bu bir SAHTE yedek dosyasıdır.\nTarih: {DateTime.Now}");

                TempData["Success"] = $"✅ '{fileName}' isimli sahte yedek oluşturuldu.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "❌ Yedekleme sırasında hata oluştu: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
