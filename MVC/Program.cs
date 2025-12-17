using APP.Domain;
using APP.Models;
using APP.Services;
using CORE.APP.Services;
using CORE.APP.Services.Authentication.MVC;
using CORE.APP.Services.Files.MVC;
using CORE.APP.Services.Session.MVC;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

var trCulture = new CultureInfo("tr-TR")
{
    DateTimeFormat =
    {
        ShortDatePattern = "d/M/yyyy",
        DateSeparator = "/"
    }
};

CultureInfo.DefaultThreadCurrentCulture = trCulture;
CultureInfo.DefaultThreadCurrentUICulture = trCulture;


builder.Services.AddDbContext<DbContext, Db>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Db")));


builder.Services.AddScoped<IService<UserRequest, UserResponse>, UserService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IService<GroupRequest, GroupResponse>, GroupService>();
builder.Services.AddScoped<IService<RoleRequest, RoleResponse>, RoleService>();
builder.Services.AddScoped<IService<CountryRequest, CountryResponse>, CountryService>();
builder.Services.AddScoped<IService<CityRequest, CityResponse>, CityService>();

builder.Services.AddHttpContextAccessor();


builder.Services.AddScoped<ICookieAuthService, CookieAuthService>();


builder.Services.AddScoped<SessionServiceBase, SessionService>();

// Register UserCartService as a scoped dependency for IUserCartService.
// Scoped lifetime ensures a new instance per HTTP request.
// UserCartService handles user cart management.
// This service registration enables constructor injection of IUserCartService to the controllers or services throughout the application.
builder.Services.AddScoped<IUserCartService, UserCartService>();

// File service:
builder.Services.AddScoped<FileServiceBase, FileService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // changed from /Users/Login to /Login since route was changed for the action
        options.AccessDeniedPath = "/Login"; // changed from /Users/Login to /Login since route was changed for the action
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
    });


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();