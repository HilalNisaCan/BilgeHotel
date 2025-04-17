namespace Project.MvcUI.Models.PureVm.ResponseModel.User
{
    public class UserRegistrationResponse
    {
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
