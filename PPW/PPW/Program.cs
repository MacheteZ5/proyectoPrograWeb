using Microsoft.EntityFrameworkCore;
using NToastNotify;
using PPW.Models;
using PPW.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Index}/{id?}");

app.UseNToastNotify();

app.MapRazorPages();

app.MapHub<ChatHub>("/chatHub");

app.Run();
