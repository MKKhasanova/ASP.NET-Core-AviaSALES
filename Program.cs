using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using NuGet.LibraryModel;
using System.Net.Sockets;
using Онлайн_билеты.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
var builder = WebApplication.CreateBuilder(args);
string connStr = "Server = (localdb)\\mssqllocaldb; Database = ABilet; Trusted_Connection = true";
//string connStr = string connStr = workstation id=Library24.mssql.somee.com;packet size=4096;user id=L_Lawlite24;pwd=Dr_Baltar1!;data source=Library24.mssql.somee.com;persist security info=False;initial catalog=Library24;TrustServerCertificate=True
// Добавление сервисов
builder.Services.AddDbContext<BiletsContext>(
options => options.UseSqlServer(connStr));
builder.Services.AddMvc();


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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
