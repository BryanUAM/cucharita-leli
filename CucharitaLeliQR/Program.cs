using CucharitaLeliQR.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<SodaContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

var forwardedHeadersOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};

forwardedHeadersOptions.KnownNetworks.Clear();
forwardedHeadersOptions.KnownProxies.Clear();

app.UseForwardedHeaders(forwardedHeadersOptions);

// PROBAR Y ACTUALIZAR BD AL INICIAR
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SodaContext>();

    try
    {
        db.Database.Migrate();
        Console.WriteLine("Migraciones aplicadas correctamente.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("ERROR AL MIGRAR BD: " + ex.ToString());
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// RUTA SIMPLE DE PRUEBA
app.MapGet("/ping", () => "OK");

// RUTA SIMPLE PARA PROBAR SI LA BD RESPONDE
app.MapGet("/test-db", async (SodaContext db) =>
{
    try
    {
        var total = await db.Clientes.CountAsync();
        return Results.Ok($"Conexión OK. Total clientes: {total}");
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.ToString());
    }
});

app.MapGet("/ping", () => "OK DESDE NUEVO DEPLOY");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Clientes}/{action=Dashboard}/{id?}");

app.Run();