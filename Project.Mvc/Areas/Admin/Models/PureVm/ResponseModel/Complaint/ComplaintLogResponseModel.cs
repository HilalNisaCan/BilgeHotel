using Project.Entities.Enums;

namespace Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Complaint
{
    public class ComplaintLogResponseModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public DateTime SubmittedDate { get; set; }
        public ComplaintStatus Status { get; set; }
        public string Response { get; set; }
        public bool IsResolved { get; set; }
    }
}
