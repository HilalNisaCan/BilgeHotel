namespace Project.MvcUI.Areas.Reservation.Models.PureVm.ResponseModel.ExtraExpense
{
    public class ExtraExpenseModel
    {
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
