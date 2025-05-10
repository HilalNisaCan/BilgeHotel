using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Managers.Abstracts;
using Project.Entities.Enums;
using Project.MvcUI.Areas.Reservation.Models.PageVm;
using Microsoft.EntityFrameworkCore;
using Project.MvcUI.Areas.Reservation.Models.PureVm.ResponseModel.ExtraExpense;
using Project.BLL.DtoClasses;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.BLL.Managers.Concretes;
using Microsoft.AspNetCore.Authorization;


namespace Project.MvcUI.Areas.Reservation.Controllers
{

/*"CheckInOutController, rezervasyon modülünde günlük giriş/çıkış kontrolü, ekstra harcama yönetimi ve çıkış tamamlama işlemlerini yürütür. 
 * Tüm işlemler katmanlı mimariye uygun şekilde Entity → DTO → ViewModel akışı ile gerçekleştirilir. 
 * Kodda var kullanılmadan açık tiplerle temiz yapı korunmuş, AutoMapper ile tüm dönüşümler otomatikleştirilmiştir.
 * Kullanıcı arayüzünde dinamik API destekli ürün seçimleri ile etkileşimli bir deneyim sağlanır."*/


    [Area("Reservation")]
    public class CheckInOutController : Controller
    {
        private readonly IReservationManager _reservationManager;
        private readonly IExtraExpenseManager _extraExpenseManager;
        private readonly IProductManager _productManager;
        private readonly IMapper _mapper;

        public CheckInOutController(IReservationManager reservationManager, IExtraExpenseManager extraExpenseManager, IMapper mapper, IProductManager productManager)
        {
            _reservationManager = reservationManager;
            _extraExpenseManager = extraExpenseManager;
            _mapper = mapper;
            _productManager = productManager;
        }

        /// <summary>
        /// Bugün giriş/çıkış yapacak rezervasyonları listeler (Entity → DTO → ViewModel).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            DateTime today = DateTime.Today;

            List< Project.Entities.Models.Reservation> allReservations = await _reservationManager.GetAllWithIncludeAsync(
                x => (x.StartDate.Date == today || x.EndDate.Date == today) && x.ReservationStatus != ReservationStatus.Completed,
                q => q.Include(x => x.Customer).ThenInclude(c => c.User).Include(x => x.Room)
            );

            List<ReservationCheckInOutModel> vm = _mapper.Map<List<ReservationCheckInOutModel>>(allReservations);
            return View(vm);
        }


        /// <summary>
        /// Çıkış yapacak rezervasyonun detaylarını getirir (DTO → ViewModel).
        /// </summary>
        [HttpGet("Complete")]
        public async Task<IActionResult> Complete(int reservationId)
        {
            ReservationDto reservation = await _reservationManager.GetWithIncludeAsync(reservationId);
            if (reservation == null) return NotFound();

            CheckOutDetailModel vm = _mapper.Map<CheckOutDetailModel>(reservation);

            if (reservation.Package != ReservationPackage.AllInclusive)
            {
                List<ExtraExpenseDto> expenses = await _extraExpenseManager.GetExpensesByReservationAsync(reservationId);
                vm.ExtraExpenses = _mapper.Map<List<ExtraExpenseModel>>(expenses);
            }

            return View(vm);
        }

        /// <summary>
        /// Çıkış işlemini onaylar.
        /// </summary>
        [HttpPost("Complete")]
        public async Task<IActionResult> CompleteConfirmed(int reservationId)
        {
            try
            {
                await _reservationManager.CompleteReservationAsync(reservationId);
                TempData["Success"] = "✅ Çıkış işlemi başarıyla tamamlandı.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Çıkış işlemi hatası: {ex.Message}");
                TempData["Error"] = "Bir hata oluştu. Lütfen tekrar deneyin.";
                return RedirectToAction("Complete", new { reservationId });
            }
        }

        /// <summary>
        /// Ekstra harcama ekleme formunu getirir.
        /// </summary>
        [HttpGet("AddExpense")]
        public IActionResult AddExpense(int reservationId)
        {
            // 🔹 Enum'dan kategori listesini SelectListItem olarak hazırla
            List<SelectListItem> categories = Enum.GetValues(typeof(ProductCategory))
                .Cast<ProductCategory>()
                .Select(c => new SelectListItem
                {
                    Value = ((int)c).ToString(),
                    Text = c.ToString()
                }).ToList();

            // 🔹 Modeli doldur ve View'a gönder
            AddExtraExpenseModel model = new AddExtraExpenseModel
            {
                ReservationId = reservationId,
                CategoryList = categories
            };

            return View(model);
        }

        /// <summary>
        /// Ekstra harcamayı ekler (RequestModel → DTO AutoMapper kullanımı)
        /// </summary>
        /// <summary>
        /// Ekstra harcamayı ekler (RequestModel → DTO AutoMapper kullanımı)
        /// </summary>
        [HttpPost("AddExpense")]
        public async Task<IActionResult> AddExpense(AddExtraExpenseModel model)
        {
            if (!ModelState.IsValid)
            {
                // ✅ Kategori listesi yeniden yüklenmeli
                model.CategoryList = Enum.GetValues(typeof(ProductCategory))
                    .Cast<ProductCategory>()
                    .Select(c => new SelectListItem
                    {
                        Value = ((int)c).ToString(),
                        Text = c.ToString()
                    }).ToList();

                return View(model);
            }

            // ✅ Ürün bilgisi çekiliyor
            ProductDto product = await _productManager.GetByIdAsync(model.ProductId);
            if (product == null)
            {
                ModelState.AddModelError("", "Ürün bulunamadı.");
                return View(model);
            }

            // ✅ İlgili rezervasyon üzerinden müşteri ID’si alınır
            ReservationDto reservation = await _reservationManager.GetWithIncludeAsync(model.ReservationId);
            if (reservation == null)
            {
                ModelState.AddModelError("", "Rezervasyon bulunamadı.");
                return View(model);
            }

            int customerId = reservation.CustomerId;
            decimal total = product.Price * model.Quantity;

            // ✅ DTO hazırlanıyor
            ExtraExpenseDto dto = new ExtraExpenseDto
            {
                CustomerId = customerId,
                ReservationId = model.ReservationId,
                ProductId = model.ProductId,
                Quantity = model.Quantity,
                UnitPrice = product.Price,
                Description = product.Name,
                ExpenseDate = model.ExpenseDate
            };

            // ✅ Veritabanına kaydediliyor
            await _extraExpenseManager.AddAsync(dto);

            // ✅ Log ve geri dönüş
            Console.WriteLine($"🧾 Ekstra Harcama Eklendi → MüşteriId: {customerId}, Ürün: {product.Name}, Tutar: {total}");

            TempData["Success"] = "Ekstra harcama başarıyla eklendi.";
            return RedirectToAction("Complete", new { reservationId = model.ReservationId });
        }


        /// <summary>
        /// Seçilen kategoriye göre ürün listesini API ile döner.
        /// </summary>
        [HttpGet("api/product/byCategory/{category}")]
        public async Task<IActionResult> GetProductsByCategory(int category)
        {
            // 🔹 Belirli kategoriye ait ürünleri getir
            List<ProductDto> products = await _productManager.GetByCategoryAsync((ProductCategory)category);

            // 🔹 Sadece gerekli alanları içeren anonim tipli liste oluştur
            IEnumerable<object> simplified = products.Select(p => new
            {
                id = p.Id,
                name = p.Name,
                unitPrice = p.Price
            });

            return Ok(simplified);
        }
    }
}
