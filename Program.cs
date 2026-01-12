using Microsoft.EntityFrameworkCore;
using SistemaAcademia.Models;
var builder = WebApplication.CreateBuilder(args);

// Define a string de conexão 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=SistemaAcademia.db";

// Registra o serviço do DbContext para usar o SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
