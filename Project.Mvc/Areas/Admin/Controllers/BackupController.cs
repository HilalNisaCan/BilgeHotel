using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Project.MvcUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class BackupController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public BackupController(IWebHostEnvironment env)
        {
            _env = env;
        }


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



        // ✅ Yedekleme işlemi
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

                using (var stream = new FileStream(filePath, FileMode.Create))
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
        [HttpPost]
        public async Task<IActionResult> BackupDatabase()
        {
            try
            {
                string backupFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "DatabaseBackups");

                if (!Directory.Exists(backupFolder))
                    Directory.CreateDirectory(backupFolder);

                // 📄 Sahte yedek dosyası oluşturuluyor
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
