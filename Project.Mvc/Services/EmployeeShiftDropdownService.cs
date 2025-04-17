using Microsoft.AspNetCore.Mvc.Rendering;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;

namespace Project.MvcUI.Services
{
    /// <summary>
    /// 📌 EmployeeShiftDropdownService
    /// Bu servis, çalışan ve vardiya dropdown verilerini controller'lara tekrar etmeden merkezi olarak sağlar.
    /// 
    /// 🎯 Amaç:
    /// - Tekrarlı ViewBag işlemlerini merkezi bir servise çekmek
    /// - Controller'ları sade ve okunabilir tutmak
    /// - MVC yapısında separation of concerns (sorumluluk ayrımı) sağlamak
    /// 
    /// 💡 Neden Servis Olarak Açıldı?
    /// - Aynı çalışan ve vardiya dropdown listeleri birden fazla controller'da kullanılıyor.
    /// - SOLID prensiplerine uygun: Tek sorumluluk prensibi (SRP) destekleniyor.
    /// - ViewModel hazırlığı, veri çekimi ve ViewBag hazırlığı gibi tekrar eden işlemleri soyutladık.
    /// 
    /// 🔐 Kullanım Yeri:
    /// - MVC katmanında sadece Admin tarafında kullanılsa da, gelecekte başka alanlar için de genişletilebilir.
    /// 
    ///
    /// "Controller’lar tekrarlayan dropdown verilerini yönetmek zorunda kalmasın diye ayrı bir UI servisi oluşturdum. 
    /// Bu sayede controller'lar sadece iş akışıyla ilgileniyor, veri hazırlama servise bırakıldı."
    /// </summary>


    //“Bu servis, controller’ların sadece iş akışıyla ilgilenmesini sağlıyor. ViewBag hazırlığı gibi tekrar eden işleri bu servis üzerinden yönettim. Böylece hem kod tekrarını azalttım hem de MVC katmanında sorumlulukları ayırmış oldum.”

    /// <summary>
    /// Vardiya atama ekranlarında dropdown listelerini (çalışanlar ve vardiyalar) getiren yardımcı servis.
    /// </summary>
    public class EmployeeShiftDropdownService
    {
        private readonly IEmployeeManager _employeeManager;
        private readonly IEmployeeShiftManager _shiftManager;

        /// <summary>
        /// Gerekli bağımlılıkları constructor ile alır.
        /// </summary>
        public EmployeeShiftDropdownService(
            IEmployeeManager employeeManager,
            IEmployeeShiftManager shiftManager)
        {
            _employeeManager = employeeManager;
            _shiftManager = shiftManager;
        }

        /// <summary>
        /// Çalışanlar ve vardiyalar için dropdown listeleri hazırlar.
        /// </summary>
        /// <returns>Çalışan ve vardiya SelectList'leri tuple olarak döner.</returns>
        public async Task<(SelectList Employees, SelectList Shifts)> GetDropdownsAsync()
        {
            List<EmployeeDto> employeeList = await _employeeManager.GetAllEmployeesAsync();
            List<EmployeeShiftDto> shiftList = await _shiftManager.GetAllAsync();

            var employeeSelectList = new SelectList(employeeList.Select(x => new
            {
                x.Id,
                FullName = $"{x.FirstName} {x.LastName}"
            }), "Id", "FullName");

            // 🆕 ShiftDisplay içine tarih bilgisi eklendi
            var shiftSelectList = new SelectList(shiftList.Select(x => new
            {
                x.Id,
                ShiftDisplay = $"{x.ShiftDate:dd.MM.yyyy} - {x.ShiftStart:hh\\:mm} - {x.ShiftEnd:hh\\:mm}"
            }), "Id", "ShiftDisplay");

            return (employeeSelectList, shiftSelectList);
        }
    }
}
