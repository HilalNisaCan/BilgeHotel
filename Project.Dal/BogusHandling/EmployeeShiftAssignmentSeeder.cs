using Microsoft.EntityFrameworkCore;
using Project.Dal.ContextClasses;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.BogusHandling
{/// <summary>
 /// EmployeeShiftAssignmentSeeder, çalışanlara uygun vardiyaları atar.
 /// - Resepsiyonistler: 6 kişi 3 vardiyaya 2'şer kişi olarak (1 kişi izinli)
 /// - Diğer personel: Sadece gündüz ve akşam vardiyası
 /// - IT ve Elektrikçi: 08:00–18:00 sabit vardiya
 /// ✅ Artık IT ve Elektrikçi için özel 08:00–18:00 vardiyası doğru şekilde atanacaktır.
 /// </summary>
    public static class EmployeeShiftAssignmentSeeder
    {
        public static async Task SeedAsync(MyContext context)
        {
            if (context.EmployeeShiftAssignments.Any()) return;

            List<Employee> employees = await context.Employees.Include(e => e.User).ToListAsync();
            List<EmployeeShift> shifts = await context.EmployeeShifts.ToListAsync();
            List<EmployeeShiftAssignment> assignments = new List<EmployeeShiftAssignment>();

            DateTime now = DateTime.Now;

            foreach (Employee employee in employees)
            {
                // IT veya Elektrikçi (tekli çalışanlar)
                if (employee.Position == EmployeePosition.ITSpecialist || employee.Position == EmployeePosition.Electrician)
                {
                    // IT ve Elektrikçi için özel 08:00–18:00 vardiyası
                    EmployeeShift customShift = new EmployeeShift
                    {
                        ShiftType = ShiftType.Daytime,
                        ShiftDate = DateTime.Today,
                        ShiftStart = new TimeSpan(8, 0, 0),
                        ShiftEnd = new TimeSpan(18, 0, 0),
                        HasOvertime = true,
                        OvertimePay = 500,
                        IsDayOff = false,
                        Description = "IT & Elektrikçi için özel 08:00–18:00 vardiyası",
                        CreatedDate = now,
                        Status = DataStatus.Inserted
                    };

                    context.EmployeeShifts.Add(customShift);
                    await context.SaveChangesAsync(); // ID almak için kaydet

                    assignments.Add(new EmployeeShiftAssignment
                    {
                        EmployeeId = employee.Id,
                        EmployeeShiftId = customShift.Id,
                        AssignedDate = now,
                        ShiftStatus = ShiftStatus.Assigned,
                        Description = $"Tekli çalışan (IT/Elektrik) vardiyası: {employee.FirstName} {employee.LastName}",
                        CreatedDate = now,
                        Status = DataStatus.Inserted
                    });

                    continue;
                }

                // Resepsiyonist: 3 vardiyaya 2’şer kişi, 1 kişi izinli
                if (employee.Position == EmployeePosition.Receptionist)
                {
                    int index = employees.Where(e => e.Position == EmployeePosition.Receptionist).ToList().IndexOf(employee);
                    if (index % 7 == 6)
                        continue;

                    int shiftIndex = index % 3;
                    var shift = shifts.Skip(shiftIndex).FirstOrDefault();
                    if (shift != null)
                    {
                        assignments.Add(new EmployeeShiftAssignment
                        {
                            EmployeeId = employee.Id,
                            EmployeeShiftId = shift.Id,
                            AssignedDate = now,
                            ShiftStatus = ShiftStatus.Assigned,
                            Description = $"Resepsiyonist vardiya ataması: {employee.FirstName} {employee.LastName} → {shift.ShiftType}",
                            CreatedDate = now,
                            Status = DataStatus.Inserted
                        });
                    }
                    continue;
                }

                // Diğer personel (Aşçı, Temizlik, Garson vb.)
                if (employee.Position != EmployeePosition.Receptionist &&
                    employee.Position != EmployeePosition.ITSpecialist &&
                    employee.Position != EmployeePosition.Electrician)
                {
                    int shiftIndex = (employee.Id % 2 == 0) ? 0 : 1;
                    var shift = shifts.Skip(shiftIndex).FirstOrDefault();
                    if (shift != null)
                    {
                        assignments.Add(new EmployeeShiftAssignment
                        {
                            EmployeeId = employee.Id,
                            EmployeeShiftId = shift.Id,
                            AssignedDate = now,
                            ShiftStatus = ShiftStatus.Assigned,
                            Description = $"Genel personel vardiya ataması: {employee.FirstName} {employee.LastName} → {shift.ShiftType}",
                            CreatedDate = now,
                            Status = DataStatus.Inserted
                        });
                    }
                }
            }

            context.EmployeeShiftAssignments.AddRange(assignments);
            await context.SaveChangesAsync();
        }
    }
}