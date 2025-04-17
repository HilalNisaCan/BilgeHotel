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

        public async Task<IActionResult> Index()
        {
            List<EmployeeDto> employeeDtos = await _employeeManager.GetAllEmployeesWithDetailsAsync();

            // TEST: Telefonu logla
            foreach (var emp in employeeDtos)
                Console.WriteLine($"[Test] {emp.FirstName} - Tel: {emp.PhoneNumber}");

            List<EmployeeResponseModel> response = _mapper.Map<List<EmployeeResponseModel>>(employeeDtos);
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            EmployeeDto dto = await _employeeManager.GetEmployeeByIdAsync(id);
            if (dto == null) return NotFound();

            EmployeeDetailVm vm = _mapper.Map<EmployeeDetailVm>(dto);
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _employeeManager.GetEmployeeByIdAsync(id);
            if (dto == null) return NotFound();

            var vm = _mapper.Map<UpdateEmployeeRequest>(dto);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateEmployeeRequest vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var dto = _mapper.Map<EmployeeDto>(vm);
            bool updated = await _employeeManager.UpdateEmployeeAsync(dto);

            if (!updated)
                TempData["Error"] = "Güncelleme başarısız oldu.";
            else
                TempData["Success"] = "Çalışan başarıyla güncellendi.";

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _employeeManager.DeleteEmployeeAsync(id);

            if (!result)
            {
                TempData["Error"] = "Silme işlemi başarısız oldu.";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Çalışan başarıyla silindi.";
            return RedirectToAction("Index");
        }
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

            if (result > 0)
                TempData["Success"] = "Çalışan başarıyla eklendi.";
            else
                TempData["Error"] = "Çalışan eklenirken bir hata oluştu.";

            return RedirectToAction("Index");
        }

    }
}
