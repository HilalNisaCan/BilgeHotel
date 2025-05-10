using AutoMapper;
using Project.BLL.DtoClasses;
using Project.Entities.Models;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Room;

namespace Project.MvcUI.DependencyResolver.RoomTypePriceResolver
{
    /// <summary>
    /// 🧠 RoomTypePriceResolver
    /// 
    /// AutoMapper içinde özel fiyat eşlemesi yapan resolver'dır.
    /// RoomDto → RoomAdminResponseModel dönüşümünde RoomType’a göre fiyat atanmasını sağlar.
    /// 
    /// 🎯 Amaç:
    /// - Oda tipiyle eşleşen fiyatın ViewModel’e yansımasını sağlamak
    /// - API'den gelen fiyat listesini mapping sırasında kullanmak
    /// 
    /// 💡 Kullanım Yeri:
    /// - Admin panelindeki oda listesi ekranı
    /// - RoomController → AutoMapper.Map sırasında context üzerinden fiyat setleme
    /// 
    /// ⚙️ Çalışma Şekli:
    /// - `ResolutionContext.Items["RoomTypePrices"]` ile dışarıdan fiyat listesi alınır
    /// - RoomType’a göre fiyat eşlemesi yapılır
    /// </summary>
    public class RoomTypePriceResolver : IValueResolver<RoomDto, RoomAdminResponseModel, decimal>
    {
        /// <summary>
        /// RoomDto → RoomAdminResponseModel dönüşümünde gecelik fiyatı hesaplar.
        /// </summary>
        /// <param name="source">RoomDto kaynağı</param>
        /// <param name="destination">Hedef ViewModel</param>
        /// <param name="destMember">Dönüştürülecek alan</param>
        /// <param name="context">AutoMapper çözümleme bağlamı</param>
        /// <returns>RoomType’a göre eşleşen fiyat, yoksa 0</returns>
        public decimal Resolve(
            RoomDto source,
            RoomAdminResponseModel destination,
            decimal destMember,
            ResolutionContext context)
        {
            if (context.Items.TryGetValue("RoomTypePrices", out object priceListObj))
            {
                List<RoomTypePriceDto>? priceList = priceListObj as List<RoomTypePriceDto>;

                if (priceList != null)
                {
                    RoomTypePriceDto? matchedPrice = priceList
                        .FirstOrDefault(p => p.RoomType == source.RoomType);

                    return matchedPrice?.PricePerNight ?? 0;
                }
            }

            return 0;
        }
    }
}
