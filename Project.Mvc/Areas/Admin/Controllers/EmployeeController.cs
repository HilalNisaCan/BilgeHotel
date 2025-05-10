using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Managers.Abstracts;
using Project.Entities.Models;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Employee;
using Project.BLL.DtoClasses;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Employee;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Entities.Enums;

namespace Project.MvcUI.Areas.Admin.Controllers
{


    /*“EmployeeController, otel bünyesindeki tüm çalışanlara ait işlemleri yöneten yapıdır.
Controller içerisinde çalışanların listelenmesi, detay görüntüleme, oluşturma, düzenleme ve silme işlemleri tamamen DTO yapıları ve AutoMapper aracılığıyla gerçekleştirilmiştir.
Katmanlar arası geçişlerde doğrudan entity kullanılmamış, böylece Separation of Concerns, Single Responsibility ve Dependency Inversion prensiplerine uyum sağlanmıştır.
Enum’lar üzerinden pozisyon ve maaş türleri ViewBag ile View'a aktarılmıştır.”*/

 
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeManager _employeeManager;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeManager employeeManager, IMapper mapper)
        {
            _employeeManager = employeeManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Tüm çalışanları detaylarıyla listeler (DTO → ResponseModel)
        /// </summary>
        public async Task<IActionResult> Index()
        {
            List<EmployeeDto> employeeDtos = await _employeeManager.GetAllEmployeesWithDetailsAsync();

            // DEBUG: Telefonu logla
            foreach (EmployeeDto emp in employeeDtos)
                Console.WriteLine($"[Test] {emp.FirstName} - Tel: {emp.PhoneNumber}");

            List<EmployeeResponseModel> response = _mapper.Map<List<EmployeeResponseModel>>(employeeDtos);
            return View(response);
        }

        /// <summary>
        /// Belirli bir çalışanın detay bilgilerini getirir
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            EmployeeDto dto = await _employeeManager.GetEmployeeByIdAsync(id);
            if (dto == null) return NotFound();

            EmployeeDetailVm vm = _mapper.Map<EmployeeDetailVm>(dto);
            return View(vm);
        }


        /// <summary>
        /// Çalışan güncelleme formunu getirir
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            EmployeeDto? dto = await _employeeManager.GetEmployeeByIdAsync(id);
            if (dto == null) return NotFound();

            UpdateEmployeeRequest vm = _mapper.Map<UpdateEmployeeRequest>(dto);
            return View(vm);
        }


        /// <summary>
        /// Çalışan güncellemesini işler (POST)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateEmployeeRequest vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            EmployeeDto dto = _mapper.Map<EmployeeDto>(vm);
            bool updated = await _employeeManager.UpdateEmployeeAsync(dto);

            TempData[updated ? "Success" : "Error"] = updated
                ? "Çalışan başarıyla güncellendi."
                : "Güncelleme başarısız oldu.";

            return RedirectToAction("Index");
        }


        /// <summary>
        /// Çalışan silme işlemini gerçekleştirir
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _employeeManager.DeleteEmployeeAsync(id);

            TempData[result ? "Success" : "Error"] = result
                ? "Çalışan başarıyla silindi."
                : "Silme işlemi başarısız oldu.";

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Yeni çalışan ekleme formunu getirir
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Positions = Enum.GetValues(typeof(EmployeePosition))
                .Cast<EmployeePosition>()
                .Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() })
                .ToList();

            ViewBag.SalaryTypes = Enum.GetValues(typeof(SalaryType))
                .Cast<SalaryType>()
                .Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() })
                .ToList();

            return View();
        }

        /// <summary>
        /// Yeni çalışan oluşturur (DTO üzerinden işlem yapılır)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Positions = Enum.GetValues(typeof(EmployeePosition))
                    .Cast<EmployeePosition>()
                    .Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() })
                    .ToList();

                ViewBag.SalaryTypes = Enum.GetValues(typeof(SalaryType))
                    .Cast<SalaryType>()
                    .Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() })
                    .ToList();

                TempData["Error"] = "Eksik veya hatalı bilgi girdiniz.";
                return View(vm);
            }

            EmployeeDto dto = _mapper.Map<EmployeeDto>(vm);
            int result = await _employeeManager.AddEmployeeAsync(dto);

            TempData[result > 0 ? "Success" : "Error"] = result > 0
                ? "Çalışan başarıyla eklendi."
                : "Çalışan eklenirken bir hata oluştu.";

            return RedirectToAction("Index");
        }

    }
}
