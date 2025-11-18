using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using TiendaEcomerce.Data;
using TiendaEcomerce.Extensions;
using TiendaEcomerce.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services para conectar DB
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new 
    InvalidOperationException("Connection string 'DefaultConnection' fail ");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//Identity con user y roles
builder.Services.AddIdentity<TiendaEcomerce.Models.ApplicationUser,
    IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 10;
    options.Password.RequiredUniqueChars = 4;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.AllowedForNewUsers = true;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
//Configuración del Security Stamp
//refrescamos la cookie cada minuto, para actualizar contraseñas, roles o permisos
//reduce riesgos de seguridad en aplicaciones sensibles, o sesiones antiguas o viejas
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(1);
    // Esto fuerza que se revalide la cookie cada minuto
    // Muy útil si cambias roles, contraseñas o permisos dinámicamente
});
//Config cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "AuthorizationApp";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
    options.SlidingExpiration = true;
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http
    .CookieSecurePolicy.Always;
});
//Google y Facebook Authentication
builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder
        .Configuration["Authentication:Google:ClientId"]!;
        googleOptions.ClientSecret = builder
        .Configuration["Authentication:Google:ClientSecret"]!;
    })
    .AddFacebook(fbOptions =>
    {
        fbOptions.AppId = builder
        .Configuration["Authentication:Facebook:AppId"]!;
        fbOptions.AppSecret = builder
        .Configuration["Authentication:Facebook:AppSecret"]!;
    });
//Email sender
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
builder.Services.Configure<DataProtectionTokenProviderOptions>
    (options =>
{
    options.TokenLifespan = TimeSpan.FromHours(3);
});

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();




if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

#region Configuracion base comentada para los headers 
///configuramos el pipeline de la aplicacion´para deshabilitar 
/// acceso a paginas cacheadas en todo mi sistema
/*
app.Use(async (context, next) =>
{
    //VERIFICAMOS que realmente el usuario no pueda  inyectar una url en paginas cacheadas
    await next();
   
    if (context.Response.StatusCode == 200 && context.User?.Identity?.IsAuthenticated == false)
    {
        context.Response.Headers["Cache-Control"] = "no-store";
        context.Response.Headers["Pragma"] = "no-cache";
    }

});*/
#endregion

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); //antes de Authorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentityDataInitializer
        .SeedRolesAndAdminAsync(services, builder.Configuration);
}
app.Run();

