using Project.Dal.ContextClasses;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.BogusHandling
{
    /// <summary>
    /// EmployeeShiftSeeder, sistemde sabit vardiyaları oluşturur.
    /// - Sabah, Akşam ve Gece vardiyaları
    /// - IT ve Elektrikçi için özel 08:00–18:00 vardiyası
    /// </summary>
    public static class EmployeeShiftSeeder
    {
        public static async Task SeedAsync(MyContext context)
        {
            if (context.EmployeeShifts.Any()) return;

            List<EmployeeShift> shifts = new List<EmployeeShift>
        {
            new EmployeeShift
            {
                ShiftType = ShiftType.Morning,
                ShiftDate = DateTime.Today,
                ShiftStart = new TimeSpan(8, 0, 0),
                ShiftEnd = new TimeSpan(16, 0, 0),
                HasOvertime = false,
                OvertimePay = 0,
                IsDayOff = false,
                Description = "Sabah vardiyası",
                CreatedDate = DateTime.Now,
                Status = DataStatus.Inserted
            },
            new EmployeeShift
            {
                ShiftType = ShiftType.Evening,
                ShiftDate = DateTime.Today,
                ShiftStart = new TimeSpan(16, 0, 0),
                ShiftEnd = new TimeSpan(0, 0, 0),
                HasOvertime = false,
                OvertimePay = 0,
                IsDayOff = false,
                Description = "Akşam vardiyası",
                CreatedDate = DateTime.Now,
                Status = DataStatus.Inserted
            },
            new EmployeeShift
            {
                ShiftType = ShiftType.Night,
                ShiftDate = DateTime.Today,
                ShiftStart = new TimeSpan(0, 0, 0),
                ShiftEnd = new TimeSpan(8, 0, 0),
                HasOvertime = false,
                OvertimePay = 0,
                IsDayOff = false,
                Description = "Gece vardiyası",
                CreatedDate = DateTime.Now,
                Status = DataStatus.Inserted
            },
            new EmployeeShift
            {
                ShiftType = ShiftType.Morning, // özel sabah tipi kullanılabilir
                ShiftDate = DateTime.Today,
                ShiftStart = new TimeSpan(8, 0, 0),
                ShiftEnd = new TimeSpan(18, 0, 0),
                HasOvertime = true,
                OvertimePay = 250,
                IsDayOff = false,
                Description = "IT ve Elektrikçi için sabit vardiya (08:00–18:00)",
                CreatedDate = DateTime.Now,
                Status = DataStatus.Inserted
            }
        };

            context.EmployeeShifts.AddRange(shifts);
            await context.SaveChangesAsync();
        }
    }
}