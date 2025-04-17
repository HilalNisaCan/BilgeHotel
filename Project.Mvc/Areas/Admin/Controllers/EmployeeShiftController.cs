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

        // 🟢 Vardiya Oluştur
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeShiftCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Eksik bilgi girdiniz.";
                return View(vm);
            }

            // Saat Başlangıcı ve Bitişini Kontrol Et
            if (vm.ShiftStart >= vm.ShiftEnd)
            {
                TempData["Error"] = "Başlangıç saati bitiş saatinden büyük olamaz.";
                return View(vm);
            }

            // Mapping işlemi
            EmployeeShiftDto dto = _mapper.Map<EmployeeShiftDto>(vm);
            await _shiftManager.CreateAsync(dto);

            TempData["Success"] = "Vardiya başarıyla eklendi.";
            return RedirectToAction("Index");
        }

        // 🟡 Vardiya Atama
        [HttpGet]
        public async Task<IActionResult> Assign()
        {
            // 📥 Dropdown verilerini servisten al
            var (employees, shifts) = await _dropdownService.GetDropdownsAsync();

            // 🎯 ViewBag'e ata
            ViewBag.Employees = employees;
            ViewBag.Shifts = shifts;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Assign(EmployeeShiftAssignmentCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    Console.WriteLine($"Property: {entry.Key}, Error: {string.Join(", ", entry.Value.Errors.Select(e => e.ErrorMessage))}");
                }

                TempData["Error"] = "Bilgileri kontrol edin.";
                return RedirectToAction("Assign");
            }

            // 🔍 VARDİYA ÇAKIŞMA KONTROLÜ
            List<EmployeeShiftAssignmentDto> existingAssignments = await _assignmentManager.GetAllAsync();

            bool isConflict = existingAssignments.Any(x =>
                x.EmployeeId == vm.EmployeeId &&
                x.AssignedDate.Date == vm.AssignedDate.Date);

            if (isConflict)
            {
                TempData["Error"] = "Bu çalışan, seçilen tarihte başka bir vardiyaya zaten atanmış.";
                return RedirectToAction("Assign");
            }

            // 🟢 UYGUNSA EKLE
            EmployeeShiftAssignmentDto dto = _mapper.Map<EmployeeShiftAssignmentDto>(vm);
            await _assignmentManager.CreateAsync(dto);

            TempData["Success"] = "Vardiya başarıyla atandı.";
            return RedirectToAction("Index");
        }

        // 🔵 Fazla Mesai Takibi
        [HttpGet]
        public async Task<IActionResult> Overtime()
        {
            var model = new EmployeeShiftOvertimeQueryVm
            {
                WeekStartDate = DateTime.Today // 🔥 BUGÜNÜ OTOMATİK ATA
            };

            if (TempData["EmployeeId"] != null)
                model.EmployeeId = Convert.ToInt32(TempData["EmployeeId"]);

            if (TempData["WeekStartDate"] != null)
                model.WeekStartDate = DateTime.Parse(TempData["WeekStartDate"].ToString());

            ViewBag.Employees = new SelectList(await _employeeManager.GetAllEmployeesAsync(), "Id", "FullName");
            return View(model); // ❗️Burası önemli: boş değil, model dönüyoruz
        }

        [HttpPost]
        public async Task<IActionResult> Overtime(EmployeeShiftOvertimeQueryVm vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen geçerli bir çalışan ve tarih seçiniz.";
                return RedirectToAction("Overtime");
            }

            // ✅ Mesai hesapla
            DateTime weekEndDate = vm.WeekStartDate.AddDays(6);
            double overtimeHours = await _shiftManager.CalculateOvertimeAsync(vm.EmployeeId, vm.WeekStartDate, weekEndDate);

            // 🧠 Sakla
            TempData["EmployeeId"] = vm.EmployeeId;
            TempData["WeekStartDate"] = vm.WeekStartDate.ToString("yyyy-MM-dd");

            TempData["Success"] = $"Seçilen haftada fazla mesai: {overtimeHours} saat";
            return RedirectToAction("Overtime");
        }



        [HttpGet]
        public async Task<IActionResult> OvertimeList(int employeeId, DateTime weekStartDate)
        {
            Console.WriteLine($"GELEN PARAMETRELER >> employeeId: {employeeId}, weekStartDate: {weekStartDate:yyyy-MM-dd}");
            ViewBag.Employees = new SelectList(await _employeeManager.GetAllEmployeesAsync(), "Id", "FullName");

            if (employeeId == 0 || weekStartDate == default)
            {
                TempData["Error"] = "Lütfen çalışan ve tarih bilgilerini eksiksiz gönderin.";
                return View(new List<EmployeeShiftOvertimeListVm>());
            }

            var assignments = await _assignmentManager.GetAssignmentsForWeekAsync(employeeId, weekStartDate);

            if (assignments == null || !assignments.Any())
            {
                TempData["Error"] = "Bu tarihlerde bu çalışana ait vardiya bulunamadı.";
                return View(new List<EmployeeShiftOvertimeListVm>());
            }

            var result = assignments.Select(x =>
            {
                TimeSpan duration = x.EmployeeShift.ShiftEnd - x.EmployeeShift.ShiftStart;
                double workedHours = duration.TotalHours;
                double overtime = workedHours > 8 ? workedHours - 8 : 0;

                return new EmployeeShiftOvertimeListVm
                {
                    FirstName = x.Employee.FirstName,
                    LastName = x.Employee.LastName,
                    AssignedDate = x.AssignedDate,
                    ShiftTime = $"{x.EmployeeShift.ShiftStart:hh\\:mm} - {x.EmployeeShift.ShiftEnd:hh\\:mm}",
                    WorkedHours = workedHours,
                    OvertimeHours = overtime
                };
            }).ToList();

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Salary()
        {
            List<EmployeeDto> employees = await _employeeManager.GetAllEmployeesAsync();
            ViewBag.Employees = new SelectList(employees, "Id", "FullName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Salary(EmployeeSalaryQueryVm vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Tarih veya çalışan seçimi hatalı.";
                return RedirectToAction("Salary");
            }

            EmployeeDto employee = await _employeeManager.GetByIdAsync(vm.EmployeeId);

            // 👉 Artık hem maaş hem toplam saat geliyor
            (decimal salary, double totalWorkedHours) = await _shiftManager.CalculateSalaryAsync(vm.EmployeeId, vm.StartDate, vm.EndDate);

            double overtime = await _shiftManager.CalculateOvertimeAsync(vm.EmployeeId, vm.StartDate, vm.EndDate);

            EmployeeSalaryResultVm result = new EmployeeSalaryResultVm
            {
                EmployeeFullName = $"{employee.FirstName} {employee.LastName}",
                Year = vm.StartDate.Year,
                Month = vm.StartDate.Month,
                OvertimeHours = overtime,
                TotalWorkedHours = totalWorkedHours,
                SalaryAmount = salary
            };

            return View("SalaryResult", result);
        }

        [HttpGet]
        public async Task<IActionResult> List(DateTime? date)
        {
            List<EmployeeShiftDto> shiftList = await _shiftManager.GetAllWithAssignmentsAsync();

            if (date.HasValue)
                shiftList = shiftList.Where(x => x.ShiftDate.Date == date.Value.Date).ToList();

            // Test çıktısı (istersen sil)
            foreach (var shift in shiftList)
            {
                Console.WriteLine($"[TEST] {shift.ShiftDate:yyyy-MM-dd} → {shift.AssignedEmployees.Count} atama");
            }

            List<EmployeeShiftResponseVm> vmList = _mapper.Map<List<EmployeeShiftResponseVm>>(shiftList);
            ViewBag.FilterDate = date?.ToString("yyyy-MM-dd");
            return View(vmList);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _shiftManager.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            var vm = _mapper.Map<EmployeeShiftUpdateVm>(dto); // ViewModel'ini oluşturduysan
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeShiftUpdateVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var dto = _mapper.Map<EmployeeShiftDto>(vm);
            await _shiftManager.UpdateAsync(dto);

            TempData["Success"] = "Vardiya güncellendi.";
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            bool isDeleted = await _shiftManager.DeleteShiftByIdAsync(id);

            if (!isDeleted)
                TempData["Error"] = "Vardiya silinirken hata oluştu.";
            else
                TempData["Success"] = "Vardiya başarıyla silindi.";

            return RedirectToAction("List");
        }

    }

}
