using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using Project.BLL.DependencyResolver;
using Project.Common.Tools;
using Project.Dal.BogusHandling.SeederManager;
using Project.Dal.ContextClasses;
using Project.Entities.Models;
using Project.MvcUI.DependencyResolver;
using Project.MvcUI.Services;
using Project.MvcUI.VmMapping;
using QuestPDF.Infrastructure;
using Rotativa.AspNetCore;
using System.Globalization;



var builder = WebApplication.CreateBuilder(args);


QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
// 👉 env tanımlanmalı
var env = builder.Environment;

// ✨ Rotativa ayarı
RotativaConfiguration.Setup(env.WebRootPath, "Rotativa");

// ✅ DI Servisleri
builder.Services.AddDbContextService();
builder.Services.AddRepositoryService();
builder.Services.AddIdentityService();

builder.Services.AddManagerService();
builder.Services.AddVmMapperService(); // 
builder.Services.AddMapperService();
builder.Services.AddControllersWithViews();
builder.Services.AddServiceDependencies();
builder.Services.AddHttpClient<RoomTypePriceApiClient>();
builder.Services.AddScoped<EmployeeShiftDropdownService>();
//builder.Services.AddAutoMapper(typeof(VmMappingProfile));
// ✅ HttpClient desteğini DI konteynerine ekle
builder.Services.AddHttpClient();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // ✅ Main tarafı hedefler
    options.AccessDeniedPath = "/Account/AccessDenied"; // opsiyonel
});



var app = builder.Build();

// Kültür ayarı (ondalıklı sayıların noktayla çalışması için)
var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// 👇👇👇 SEEDER BURAYA EKLENDİ
// SEED işlemi için scope açılır
// SEED işlemi için scope açılır
using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider serviceProvider = scope.ServiceProvider;

    MyContext context = serviceProvider.GetRequiredService<MyContext>();
    UserManager<User> userManager = serviceProvider.GetRequiredService<UserManager<User>>();

    await SeederManager.SeedAllAsync(context, userManager);
} /*
   *  sahte veri seeding işlemini sadece IsDevelopment() ortamında çalışacak şekilde ekledim.
     Böylece production'da veri bozulmaz. UserManager ve DbContext’i Dependency Injection üzerinden alarak profesyonel bir yapı kurdum.
   * 
   * */



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller}/{action}/{id?}");

app.MapControllerRoute(
   name: "default",
   pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();