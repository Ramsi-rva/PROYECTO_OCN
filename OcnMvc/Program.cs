using Microsoft.EntityFrameworkCore;
using OcnMvc.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Cadena de conexión "Ocn" definida en appsettings.json
builder.Services.AddDbContext<OcnContext>(opciones =>
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("Ocn")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Levantamiento}/{action=Index}/{id?}");

app.Run();
