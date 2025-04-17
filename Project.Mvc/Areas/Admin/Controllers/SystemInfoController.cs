using Microsoft.AspNetCore.Mvc;
using Project.MvcUI.Areas.Admin.Models.PageVm;
using Rotativa.AspNetCore;

namespace Project.MvcUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SystemInfoController : Controller
    {
        // Demo: Sistem bilgisi bir ViewModel ile tutuluyor (sabit veri)
        private static SystemInfoViewModel _systemInfo = new SystemInfoViewModel
        {
            ResepsiyonBilgileriSayisi = "4 bilgisayar",
            BarBilgisayariSayisi = "1 bilgisayar",
            HavuzBilgisayariSayisi = "1 bilgisayar",
            YoneticiBilgisayariSayisi = "6 bilgisayar",
            Sunucu1Bilgisi = "Windows Server 2003 – Mail sunucusu ve Access dosyaları yüklü",
            Sunucu2Bilgisi = "Windows XP Professional – Wireless internet için, domaine bağlı değil",
            DomainDurumu = "Ana makineler domaine bağlıdır",
            WirelessDurumu = "Wireless sunucu domain dışıdır",
            ClientIsletimSistemi = "Bazı client sistemler Windows XP Home Edition",
            YedeklemeBilgisi = "Program üzerinden yetkili kullanıcı yedek alabilir",
            GeriYuklemeBilgisi = "Yalnızca yetkili kullanıcı geri yükleme yapabilir",
            EsneklikBilgisi = "Sunucu değişikliğine karşı yapı esnektir",
            Aciklama = "Program, domaine bağlı ya da bağımsız çalışabilir. Eski sistemlerle uyumludur."
        };



        public IActionResult Index()
        {
            return View(_systemInfo);
        }


        public IActionResult ExportToPdf()
        {
            return new ViewAsPdf("Index")
            {
                FileName = "SistemBilgileri.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait
            };
        }

        // ✔️ GET: Düzenleme sayfasını göster
        [HttpGet]
        public IActionResult Edit()
        {
            return View(_systemInfo);
        }

        // ✔️ POST: Kaydet
        [HttpPost]
        public IActionResult Edit(SystemInfoViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _systemInfo = model; // geçici olarak static olarak tutuyoruz

            TempData["Success"] = "Sistem bilgileri başarıyla güncellendi.";
            return RedirectToAction("Index");
        }
    }
}
