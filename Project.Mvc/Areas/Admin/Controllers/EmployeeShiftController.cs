using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Employee;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.EmployeeShift;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.EmployeeShift;
using Project.MvcUI.Services;

namespace Project.MvcUI.Areas.Admin.Controllers
{

    /*"EmployeeShiftController, çalışanların vardiya yönetimi, atamaları, fazla mesai hesaplamaları ve maaş takibi gibi operasyonel iş yüklerini yöneten gelişmiş bir yönetim modülüdür.
Tüm işlemler DTO ve ViewModel katmanları üzerinden yürütülmekte, AutoMapper ile veri dönüşümleri sağlanmakta ve UI bileşenleri (dropdownlar gibi) dinamik olarak servis üzerinden çekilmektedir.
Fazla mesai çakışma kontrolü, haftalık saat hesaplama, maaş tahmini gibi iş kuralları doğrudan Business Layer içerisinde soyutlanmıştır.
Böylece controller yalnızca arayüz ile etkileşimde bulunur.”"*/


    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class EmployeeShiftController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeShiftManager _shiftManager;
        private readonly IEmployeeShiftAssignmentManager _assignmentManager;
        private readonly IEmployeeManager _employeeManager;
        private readonly EmployeeShiftDropdownService _dropdownService;

        public EmployeeShiftController(
            IMapper mapper,
            IEmployeeShiftManager shiftManager,
            IEmployeeShiftAssignmentManager assignmentManager,
            IEmployeeManager employeeManager, EmployeeShiftDropdownService dropdownService)
        {
            _mapper = mapper;
            _shiftManager = shiftManager;
            _assignmentManager = assignmentManager;
            _employeeManager = employeeManager;
            _dropdownService = dropdownService;
        }


        // 🟦 Dashboard
        public IActionResult Index()
        {
            return View();
        }

        // 🟢 Vardiya Oluştur (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 🟢 Vardiya Oluştur (POST)
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeShiftCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Eksik bilgi girdiniz.";
                return View(vm);
            }

            if (vm.ShiftStart >= vm.ShiftEnd)
            {
                TempData["Error"] = "Başlangıç saati bitiş saatinden büyük olamaz.";
                return View(vm);
            }

            EmployeeShiftDto dto = _mapper.Map<EmployeeShiftDto>(vm);
            await _shiftManager.CreateAsync(dto);

            TempData["Success"] = "Vardiya başarıyla eklendi.";
            return RedirectToAction("Index");
        }


        // 🟡 Vardiya Atama (GET)
        [HttpGet]
        public async Task<IActionResult> Assign()
        {
            (SelectList employees, SelectList shifts) = await _dropdownService.GetDropdownsAsync();
            ViewBag.Employees = employees;
            ViewBag.Shifts = shifts;
            return View();
        }

        // 🟡 Vardiya Atama (POST)
        [HttpPost]
        public async Task<IActionResult> Assign(EmployeeShiftAssignmentCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Bilgileri kontrol edin.";
                return RedirectToAction("Assign");
            }

            List<EmployeeShiftAssignmentDto> existingAssignments = await _assignmentManager.GetAllAsync();

            bool isConflict = existingAssignments.Any(x =>
                x.EmployeeId == vm.EmployeeId &&
                x.AssignedDate.Date == vm.AssignedDate.Date);

            if (isConflict)
            {
                TempData["Error"] = "Bu çalışan, seçilen tarihte başka bir vardiyaya zaten atanmış.";
                return RedirectToAction("Assign");
            }

            EmployeeShiftAssignmentDto dto = _mapper.Map<EmployeeShiftAssignmentDto>(vm);
            await _assignmentManager.CreateAsync(dto);

            TempData["Success"] = "Vardiya başarıyla atandı.";
            return RedirectToAction("Index");
        }


        // 🔵 Fazla Mesai Takibi (GET)
        [HttpGet]
        public async Task<IActionResult> Overtime()
        {
            EmployeeShiftOvertimeQueryVm model = new EmployeeShiftOvertimeQueryVm
            {
                WeekStartDate = DateTime.Today
            };

            if (TempData["EmployeeId"] != null)
                model.EmployeeId = Convert.ToInt32(TempData["EmployeeId"]);

            if (TempData["WeekStartDate"] != null)
                model.WeekStartDate = DateTime.Parse(TempData["WeekStartDate"].ToString());

            List<EmployeeDto> employees = await _employeeManager.GetAllEmployeesAsync();
            ViewBag.Employees = new SelectList(employees, "Id", "FullName");
            return View(model);
        }

        // 🔵 Fazla Mesai Takibi (POST)
        [HttpPost]
        public async Task<IActionResult> Overtime(EmployeeShiftOvertimeQueryVm vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen geçerli bir çalışan ve tarih seçiniz.";
                return RedirectToAction("Overtime");
            }

            DateTime weekEndDate = vm.WeekStartDate.AddDays(6);
            double overtimeHours = await _shiftManager.CalculateOvertimeAsync(vm.EmployeeId, vm.WeekStartDate, weekEndDate);

            TempData["EmployeeId"] = vm.EmployeeId;
            TempData["WeekStartDate"] = vm.WeekStartDate.ToString("yyyy-MM-dd");

            TempData["Success"] = $"Seçilen haftada fazla mesai: {overtimeHours} saat";
            return RedirectToAction("Overtime");
        }



        /// <summary>
        /// Seçilen çalışanın belirli bir haftadaki vardiya listesini ve fazla mesaisini getirir.
        /// DTO → ViewModel dönüşümleri AutoMapper ile yapılır.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> OvertimeList(int employeeId, DateTime weekStartDate)
        {
            // 🔍 Gelen parametreleri kontrol için logla (isteğe bağlı)
            Console.WriteLine($"GELEN PARAMETRELER >> employeeId: {employeeId}, weekStartDate: {weekStartDate:yyyy-MM-dd}");

            // 📌 Tüm çalışanlar dropdown için
            List<EmployeeDto> employees = await _employeeManager.GetAllEmployeesAsync();
            ViewBag.Employees = new SelectList(employees, "Id", "FullName");

            // ❗ Parametre doğrulama
            if (employeeId == 0 || weekStartDate == default)
            {
                TempData["Error"] = "Lütfen çalışan ve tarih bilgilerini eksiksiz gönderin.";
                return View(new List<EmployeeShiftOvertimeListVm>());
            }

            // 📥 İlgili tarihteki vardiya atamalarını al
            List<EmployeeShiftAssignmentDto> assignments = await _assignmentManager.GetAssignmentsForWeekAsync(employeeId, weekStartDate);

            // ❌ Atama yoksa bilgilendir
            if (assignments == null || !assignments.Any())
            {
                TempData["Error"] = "Bu tarihlerde bu çalışana ait vardiya bulunamadı.";
                return View(new List<EmployeeShiftOvertimeListVm>());
            }

            // ✅ AutoMapper ile dönüşüm
            List<EmployeeShiftOvertimeListVm> result = _mapper.Map<List<EmployeeShiftOvertimeListVm>>(assignments);

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Salary()
        {
            List<EmployeeDto> employees = await _employeeManager.GetAllEmployeesAsync();
            ViewBag.Employees = new SelectList(employees, "Id", "FullName");
            return View();
        }

        /// <summary>
        /// Seçilen çalışan ve tarih aralığına göre maaş hesaplaması yapar.
        /// AutoMapper ile çalışan bilgileri ViewModel'e dönüştürülür.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Salary(EmployeeSalaryQueryVm vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Tarih veya çalışan seçimi hatalı.";
                return RedirectToAction("Salary");
            }

            // 1️⃣ Çalışan DTO'sunu getir
            EmployeeDto employee = await _employeeManager.GetByIdAsync(vm.EmployeeId);

            // 2️⃣ DTO → ViewModel dönüşümünü yap
            EmployeeSalaryResultVm result = _mapper.Map<EmployeeSalaryResultVm>(employee);

            // 3️⃣ Maaş ve mesai hesaplamaları
            (decimal salary, double totalWorkedHours) = await _shiftManager.CalculateSalaryAsync(vm.EmployeeId, vm.StartDate, vm.EndDate);
            double overtime = await _shiftManager.CalculateOvertimeAsync(vm.EmployeeId, vm.StartDate, vm.EndDate);

            // 4️⃣ ViewModel'e hesaplama sonuçlarını ekle
            result.Year = vm.StartDate.Year;
            result.Month = vm.StartDate.Month;
            result.OvertimeHours = overtime;
            result.TotalWorkedHours = totalWorkedHours;
            result.SalaryAmount = salary;

            // 5️⃣ Sonuç ekranına gönder
            return View("SalaryResult", result);
        }

        // 📅 Vardiya Listesi
        [HttpGet]
        public async Task<IActionResult> List(DateTime? date)
        {
            List<EmployeeShiftDto> shiftList = await _shiftManager.GetAllWithAssignmentsAsync();

            if (date.HasValue)
                shiftList = shiftList.Where(x => x.ShiftDate.Date == date.Value.Date).ToList();

            List<EmployeeShiftResponseVm> vmList = _mapper.Map<List<EmployeeShiftResponseVm>>(shiftList);
            ViewBag.FilterDate = date?.ToString("yyyy-MM-dd");
            return View(vmList);
        }

        // 📝 Vardiya Düzenle (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            EmployeeShiftDto? dto = await _shiftManager.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            EmployeeShiftUpdateVm vm = _mapper.Map<EmployeeShiftUpdateVm>(dto);
            return View(vm);
        }

        // 📝 Vardiya Düzenle (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeShiftUpdateVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            EmployeeShiftDto dto = _mapper.Map<EmployeeShiftDto>(vm);
            await _shiftManager.UpdateShiftAsync(dto);

            TempData["Success"] = "Vardiya güncellendi.";
            return RedirectToAction("List");
        }

        // ❌ Vardiya Sil
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            bool isDeleted = await _shiftManager.DeleteShiftByIdAsync(id);

            TempData[isDeleted ? "Success" : "Error"] = isDeleted
                ? "Vardiya başarıyla silindi."
                : "Vardiya silinirken hata oluştu.";

            return RedirectToAction("List");
        }

    }

}
