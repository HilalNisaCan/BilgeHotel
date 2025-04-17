using Microsoft.EntityFrameworkCore;
using Project.BLL.DependencyResolver;
using Project.BLL.Managers.Abstracts;
using Project.BLL.Managers.Concretes;
using Project.BLL.Services.abstracts;
using Project.BLL.Services.Concretes;
using Project.Dal.ContextClasses;
using Project.Dal.Repositories.Abstracts;
using Project.Dal.Repositories.Concretes;
using Project.WebApi.WebApiResolver;


var builder = WebApplication.CreateBuilder(args);

// ⭐ CORS politikası tanımı (Web sitesiyle haberleşme için)
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5001") // MVC projenin adresi
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// 🔧 EF Core bağlantısı
builder.Services.AddDbContext<MyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));

// 🔧 AutoMapper servisi
builder.Services.AddMapperService();

// 🔧 WebAPI için tüm Manager + Repository'leri burada ekliyoruz (temiz yapı)
builder.Services.AddWebApiResolvers();
builder.Services.AddIdentityService();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();


// 🔧 Controller servisi (API)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ⭐ Development ortamıysa Swagger aktif edilir
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ⭐ CORS aktif edilir (UI → API çağrısı için zorunlu)
app.UseCors(MyAllowSpecificOrigins);

// 🔐 Yetkilendirme (Identity burada olmasa bile default yapı)
app.UseAuthorization();

// 🌐 Controller endpoint'leri
app.MapControllers();

// 🚀 Uygulama başlatılır
app.Run();