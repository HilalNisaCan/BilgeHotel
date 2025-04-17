using System.ComponentModel.DataAnnotations;

namespace Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Complaint
{
    public class ComplaintReplyRequestModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Yanıt metni boş bırakılamaz.")]
        [StringLength(1000, ErrorMessage = "Yanıt en fazla 1000 karakter olabilir.")]
        public string Response { get; set; }
    }
}
