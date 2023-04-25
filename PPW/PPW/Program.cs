using Microsoft.EntityFrameworkCore;
using NToastNotify;
using PPW.Models;
using PPW.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(s =>
{
    s.IdleTimeout = TimeSpan.FromMinutes(30);
    s.Cookie.Name = ".MyProyect.Session"; /*You can change this variable*/
    s.Cookie.Expiration = TimeSpan.FromMinutes(30);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllOrigins",builder => { builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod(); });
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.LoginPath = "/Users"; /*this option indicates where is the login page*/
});

var connection = builder.Configuration.GetConnectionString("MySQLConnection");
builder.Services.AddDbContext<ProgramacionWebContext>(options => options.UseMySQL(connection));

builder.Services.AddRazorPages().AddNToastNotifyNoty(new NotyOptions { ProgressBar = true, Timeout = 4000 });

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Index}/{id?}");

app.UseNToastNotify();

app.MapRazorPages();

app.MapHub<ChatHub>("/chatHub");

app.Run();
