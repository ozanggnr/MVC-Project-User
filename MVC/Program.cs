using APP.Domain;
using APP.Models;
using APP.Services;
using CORE.APP.Services;
using CORE.APP.Services.Authentication.MVC;
using CORE.APP.Services.Files.MVC;
using CORE.APP.Services.Session.MVC;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DbContext, Db>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Db")));


builder.Services.AddScoped<IService<UserRequest, UserResponse>, UserService>();
builder.Services.AddScoped<IService<GroupRequest, GroupResponse>, GroupService>();
builder.Services.AddScoped<IService<RoleRequest, RoleResponse>, RoleService>();


builder.Services.AddHttpContextAccessor();


builder.Services.AddScoped<ICookieAuthService, CookieAuthService>();
builder.Services.AddScoped<SessionServiceBase, SessionService>();
builder.Services.AddScoped<FileServiceBase, FileService>();


builder.Services.AddSession(config =>
{
    config.IdleTimeout = TimeSpan.FromMinutes(30);
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; 
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
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


app.UseAuthentication(); 

app.UseAuthorization();  

app.UseSession();        

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();