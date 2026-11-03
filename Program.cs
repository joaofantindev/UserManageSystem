using CompleteSystem.Data;
using CompleteSystem.Models;
using CompleteSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManageSystem.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>(); //hash de senha
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
// AddScoped cria uma instancia a cada requisição

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddScoped<IUsuarioService, UsuarioService>();

var app = builder.Build();

// Garante que o banco exista e esteja com as migrations aplicadas.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();