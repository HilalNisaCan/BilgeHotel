using Bogus;
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
    public static class EmployeeSeeder
    {


        public static async Task SeedAsync(MyContext context)
        {
            if (!context.Employees.Any())
            {
                List<Employee> employees = GenerateEmployees();
                context.Employees.AddRange(employees);
                await context.SaveChangesAsync();
            }
        }


        public static List<Employee> GenerateEmployees()
        {
            List<Employee> employees = new List<Employee>();

            // Yönetici ve özel çalışanlar
            employees.AddRange(SpecialEmployees());

            // Sahte personeller
            employees.AddRange(CreateReceptionists());
            employees.AddRange(CreateDepartmentEmployees(EmployeePosition.Chef, 11));
            employees.AddRange(CreateDepartmentEmployees(EmployeePosition.Cleaner, 11));
            employees.AddRange(CreateDepartmentEmployees(EmployeePosition.Waiter, 13));
            employees.AddRange(CreateNormalShiftEmployee(EmployeePosition.Electrician));
            employees.AddRange(CreateNormalShiftEmployee(EmployeePosition.ITSpecialist));

            return employees;
        }

        private static List<Employee> SpecialEmployees()
        {
            return new List<Employee>
            {
                new Employee
                {
                    FirstName = "Selahattin",
                    LastName = "Alkomut",
                    PhoneNumber = "05001112233",
                    Address = "İstanbul",
                    IdentityNumber = "12345678901",
                    Position = EmployeePosition.HumanResourcesManager,
                    SalaryType = SalaryType.Monthly,
                    MonthlySalary = 65000,
                    HourlyWage = 0, // Bu şekilde net belirt
                    ShiftStart = new TimeSpan(8, 0, 0),
                    ShiftEnd = new TimeSpan(17, 0, 0),
                    HasOvertime = true,
                    WeeklyWorkedHours = 45,
                    TotalWorkedHours = 1800,
                    IsActive = true,
                    HireDate = DateTime.Now.AddYears(-2),
                    CreatedDate = DateTime.Now,
                    Status = DataStatus.Inserted,
                    WeeklyOffDay = "Cuma" // 👈 eklendi
                  
                },
                new Employee
                {
                    FirstName = "Levent",
                    LastName = "Sişarpsoy",
                    PhoneNumber = "05003334455",
                    Address = "Ankara",
                    IdentityNumber = "12345678902",
                    Position = EmployeePosition.SalesManager,
                     SalaryType = SalaryType.Monthly,
                    MonthlySalary = 65000,
                    ShiftStart = new TimeSpan(9, 0, 0),
                    ShiftEnd = new TimeSpan(18, 0, 0),
                    HasOvertime = false,
                    WeeklyWorkedHours = 40,
                    TotalWorkedHours = 1600,
                    IsActive = true,
                    HireDate = DateTime.Now.AddYears(-1),
                    CreatedDate = DateTime.Now,
                    Status = DataStatus.Inserted,
                    WeeklyOffDay = "Pazartesi"

                },
                new Employee
                {
                    FirstName = "Gülay",
                    LastName = "Aydınlık",
                    PhoneNumber = "05005556677",
                    Address = "İzmir",
                    IdentityNumber = "12345678903",
                    Position = EmployeePosition.ReceptionChief,
                    SalaryType = SalaryType.Monthly,
                    MonthlySalary = 65000,
                    ShiftStart = new TimeSpan(7, 0, 0),
                    ShiftEnd = new TimeSpan(15, 0, 0),
                    HasOvertime = true,
                    WeeklyWorkedHours = 44,
                    TotalWorkedHours = 1700,
                    IsActive = true,
                    HireDate = DateTime.Now.AddYears(-3),
                    CreatedDate = DateTime.Now,
                    Status = DataStatus.Inserted,
                    WeeklyOffDay = "Çarşamba"

                },
                new Employee
                {
                    FirstName = "Selahattin",
                    LastName = "Karadibag",
                    PhoneNumber = "05009998877",
                    Address = "Bursa",
                    IdentityNumber = "12345678904",
                    Position = EmployeePosition.ITSpecialist,
                     SalaryType = SalaryType.Monthly,
                    MonthlySalary = 65000,
                    ShiftStart = new TimeSpan(10, 0, 0),
                    ShiftEnd = new TimeSpan(19, 0, 0),
                    HasOvertime = true,
                    WeeklyWorkedHours = 46,
                    TotalWorkedHours = 1850,
                    IsActive = true,
                    HireDate = DateTime.Now.AddYears(-1),
                    CreatedDate = DateTime.Now,
                    Status = DataStatus.Inserted,
                    WeeklyOffDay = "Pazar"

                }
            };
        }

        private static List<Employee> CreateReceptionists()
        {
            List<Employee> receptionists = new List<Employee>();
            Faker faker = new Faker("en");

            // 3 vardiya: 08-16, 16-00, 00-08 — her vardiyada 2 kişi, toplam 6 kişi
            TimeSpan[] shiftStarts = { new TimeSpan(8, 0, 0), new TimeSpan(16, 0, 0), new TimeSpan(0, 0, 0) };
            TimeSpan[] shiftEnds = { new TimeSpan(16, 0, 0), new TimeSpan(0, 0, 0), new TimeSpan(8, 0, 0) };

            int id = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 2; j++) // her vardiyada 2 kişi
                {
                    receptionists.Add(GenerateBasicEmployee(faker, EmployeePosition.Receptionist, shiftStarts[i], shiftEnds[i]));
                    id++;
                }
            }

            // 7. kişi izinli (vardiyasız)
            Employee offDayReceptionist = GenerateBasicEmployee(faker, EmployeePosition.Receptionist, TimeSpan.Zero, TimeSpan.Zero);
            offDayReceptionist.IsActive = false;
            receptionists.Add(offDayReceptionist);

            return receptionists;
        }

        private static List<Employee> CreateDepartmentEmployees(EmployeePosition position, int count)
        {
            List<Employee> departmentEmployees = new List<Employee>();
            Faker faker = new Faker("en");

            for (int i = 0; i < count; i++)
            {
                TimeSpan shiftStart = i % 2 == 0 ? new TimeSpan(8, 0, 0) : new TimeSpan(16, 0, 0);
                TimeSpan shiftEnd = i % 2 == 0 ? new TimeSpan(16, 0, 0) : new TimeSpan(0, 0, 0);

                departmentEmployees.Add(GenerateBasicEmployee(faker, position, shiftStart, shiftEnd));
            }

            return departmentEmployees;
        }

        private static List<Employee> CreateNormalShiftEmployee(EmployeePosition position)
        {
            Faker faker = new Faker("en");
            List<Employee> list = new List<Employee>();

            Employee employee = GenerateBasicEmployee(faker, position, new TimeSpan(8, 0, 0), new TimeSpan(18, 0, 0));
            employee.HasOvertime = faker.Random.Bool(); // bazen ek mesai
            list.Add(employee);

            return list;
        }

        private static Employee GenerateBasicEmployee(Faker faker, EmployeePosition position, TimeSpan start, TimeSpan end)
        {
            return new Faker<Employee>("en")
         .RuleFor(e => e.FirstName, f => f.Name.FirstName())
         .RuleFor(e => e.LastName, f => f.Name.LastName())
         .RuleFor(e => e.PhoneNumber, f => f.Phone.PhoneNumber("05#########"))
         .RuleFor(e => e.Address, f => f.Address.FullAddress())
         .RuleFor(e => e.IdentityNumber, f => f.Random.Replace("###########"))
         .RuleFor(e => e.Position, position)
         .RuleFor(e => e.SalaryType, SalaryType.Hourly)
         .RuleFor(e => e.HourlyWage, f => f.Random.Decimal(80, 150))
         .RuleFor(e => e.MonthlySalary, 0m) // 👈 decimal 0 olarak net belirtiyoruz
         .RuleFor(e => e.ShiftStart, start)
         .RuleFor(e => e.ShiftEnd, end)
         .RuleFor(e => e.HasOvertime, false)
         .RuleFor(e => e.WeeklyWorkedHours, f => f.Random.Int(35, 50))
         .RuleFor(e => e.TotalWorkedHours, f => f.Random.Int(500, 2000))
         .RuleFor(e => e.IsActive, true)
         .RuleFor(e => e.HireDate, f => f.Date.Past(2))
         .RuleFor(e => e.CreatedDate, DateTime.Now)
         .RuleFor(e => e.Status, DataStatus.Inserted)
         .RuleFor(e => e.WeeklyOffDay, f => f.PickRandom(new[] { "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi", "Pazar" }))
         .Generate();
        }

    }


    /*“Yöneticiler tek tek elle girildi, çünkü sistemde özel rolleri var. Diğer çalışanlar ise pozisyon bazlı sayılarla faker ile üretildi.”

“Çalışanlar için pozisyon, vardiya saatleri, maaş türü, mesai bilgileri gibi tüm alanlar sahte ama gerçekçi şekilde doldu.”

“Yönetici değişmez ama garson sayısı arttırılabilir — bu yüzden bu yapı esnek tutuldu.”

*/
}
