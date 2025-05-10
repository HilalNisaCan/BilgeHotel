using Project.Entities.Models;

namespace Project.MvcUI.Models.PageVm.User
{
    public class UserProfilePageVm
    {
        public UserProfile Profile { get; set; }
        public List<Project.Entities.Models.Reservation> PastReservations { get; set; }
        public List<Project.Entities.Models.Reservation> CurrentReservations { get; set; }

    }
}
