namespace Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.RoomTypePrice
{
    /// <summary>
    /// Oda tipi fiyatının kullanıcıya gösterilecek versiyonu (string tip açıklama içerir).
    /// </summary>
    public class RoomTypePriceResponseModel
    {
        public int Id { get; set; }

        /// <summary>
        /// Oda tipi ismi (örneğin: Double, Suite vs.)
        /// </summary>
        public string RoomTypeName { get; set; }

        /// <summary>
        /// Gecelik fiyat
        /// </summary>
        public decimal PricePerNight { get; set; }
    }
}
