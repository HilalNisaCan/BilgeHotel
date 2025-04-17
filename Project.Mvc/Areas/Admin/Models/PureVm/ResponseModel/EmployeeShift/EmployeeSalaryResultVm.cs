namespace Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.EmployeeShift
{
    public class EmployeeSalaryResultVm
    {
        public string EmployeeFullName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public double TotalWorkedHours { get; set; }
        public double OvertimeHours { get; set; }
        public decimal SalaryAmount { get; set; }
    }
}
