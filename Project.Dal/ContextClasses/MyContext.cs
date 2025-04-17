using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Project.Conf.Options;
using System.Reflection.Emit;

namespace Project.Dal.ContextClasses
{
    public class MyContext: IdentityDbContext<User, IdentityRole<int>, int>
    {
        public MyContext(DbContextOptions<MyContext> opt) : base(opt)
        {

        }

        public MyContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new UserProfileConfiguration());
            builder.ApplyConfiguration(new EmployeeConfiguration());
            builder.ApplyConfiguration(new EmployeeShiftConfiguration());
            builder.ApplyConfiguration(new RoomConfiguration());
            builder.ApplyConfiguration(new RoomMaintenanceConfiguration());
            builder.ApplyConfiguration(new ReservationConfiguration());
            builder.ApplyConfiguration(new PaymentConfiguration());
            builder.ApplyConfiguration(new ExtraExpenseConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
            builder.ApplyConfiguration(new OrderConfiguration());
            builder.ApplyConfiguration(new OrderDetailConfiguration());
            builder.ApplyConfiguration(new CampaignConfiguration());
            builder.ApplyConfiguration(new ReviewConfiguration());
            builder.ApplyConfiguration(new ReportLogConfiguration());
            builder.ApplyConfiguration(new RoomMaintenanceAssignmentConfiguration());
            builder.ApplyConfiguration(new ExchangeRateConfiguration());
            builder.ApplyConfiguration(new EmployeeShiftAssignmentConfiguration());
            builder.ApplyConfiguration(new RoomImageConfiguration());
            builder.ApplyConfiguration(new CustomerConfiguration());
            builder.ApplyConfiguration(new EarlyReservationDiscountConfiguration());
            builder.ApplyConfiguration(new BackupLogConfiguration());
            builder.ApplyConfiguration(new ComplaintLogConfiguration());
            builder.ApplyConfiguration(new GuestVisitLogConfiguration());
            builder.ApplyConfiguration(new RoomTypePriceConfiguration());  // Burada yapılandırmayı uyguluyoruz

        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeShift> EmployeeShifts { get; set; }
        public DbSet<EmployeeShiftAssignment> EmployeeShiftAssignments { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomMaintenance> RoomMaintenances { get; set; }
        public DbSet<RoomMaintenanceAssignment> RoomMaintenanceAssignments { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ExtraExpense> ExtraExpenses { get; set; }
        public DbSet<ReportLog> ReportLogs { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<EarlyReservationDiscount> EarlyReservationDiscounts { get; set; }
        public DbSet<BackupLog> BackupLogs { get; set; }
        public DbSet<RoomCleaningSchedule> RoomCleaningSchedules { get; set; }
        public DbSet<ComplaintLog> ComplaintLogs { get; set; }
        public DbSet<GuestVisitLog> GuestVisitLogs { get; set; }
        public DbSet<RoomTypePrice> RoomTypePrices { get; set; }

    }
    
}
