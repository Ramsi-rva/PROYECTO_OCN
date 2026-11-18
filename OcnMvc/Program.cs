using OcnMvc.Datos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Capa de datos: una instancia por petición, con la cadena "Ocn" de appsettings.json.
string cadenaConexion = builder.Configuration.GetConnectionString("Ocn")
    ?? throw new InvalidOperationException("No se encontró la cadena 'Ocn' en appsettings.json.");

builder.Services.AddScoped(_ => new ZonaDatos(cadenaConexion));
builder.Services.AddScoped(_ => new ProblematicaDatos(cadenaConexion));
builder.Services.AddScoped(_ => new RegistroOCNDatos(cadenaConexion));
builder.Services.AddScoped(_ => new CatalogoDatos(cadenaConexion));

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
    pattern: "{controller=RegistrosOCN}/{action=Index}/{id?}");

app.Run();
