using AutoMapper;
using Project.BLL.DtoClasses;
using Project.Entities.Models;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Room;

namespace Project.MvcUI.DependencyResolver.RoomTypePriceResolver
{
    public class RoomTypePriceResolver
     : IValueResolver<RoomDto, RoomAdminResponseModel, decimal>
    {
        public decimal Resolve(
            RoomDto source,
            RoomAdminResponseModel destination,
            decimal destMember,
            ResolutionContext context)
        {
            if (context.Items.TryGetValue("RoomTypePrices", out object priceListObj))
            {
                var priceList = priceListObj as List<RoomTypePriceDto>;
                if (priceList != null)
                {
                    return priceList.FirstOrDefault(p => p.RoomType == source.RoomType)?.PricePerNight ?? 0;
                }
            }

            return 0;
        }
    }
}
