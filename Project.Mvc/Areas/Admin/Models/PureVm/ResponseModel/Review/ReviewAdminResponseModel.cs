using Project.Entities.Enums;

namespace Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Review
{
    public class ReviewAdminResponseModel
    {
        public int Id { get; set; }
        public string UserFullName { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime CommentDate { get; set; }
        public bool IsApproved { get; set; }
        public RoomType RoomType { get; set; }
        public string? UserEmail { get; set; }
    }
}
