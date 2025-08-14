using AgencijaZaNekretnine.Data;
using AgencijaZaNekretnine.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>    ///<ApplicationDbContext>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<AgencijaNekretninaContext>(options =>    
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>().AddErrorDescriber<CustomIdentityErrorDescriber>();
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages().
    AddRazorRuntimeCompilation();

builder.Services.AddControllersWithViews().AddRazorPagesOptions(options => {
    options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "");
});

/*builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, options =>
{
    options.LoginPath = "/LogIn";
});*/


/*services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/auth/login";
    options.AccessDeniedPath = "/auth/accessdenied";
    options.Cookie.IsEssential = true;
    options.SlidingExpiration = true; // here 1
    options.ExpireTimeSpan = TimeSpan.FromSeconds(10);// here 2
});*/

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = new PathString("/Dashboard/AccessDeniedPage");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseDefaultFiles();



app.UseAuthentication();
app.UseAuthorization();


/*app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Nekretnina}/{action=Index}/{id?}");*/

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Rezervacija}/{action=Index}/{id?}");

/*app.MapControllerRoute(name: "rezervacija", pattern: "{fileName?}",
    defaults: new { controller = "Rezervacija", action = "Index" });*/

app.MapRazorPages();

app.MapControllers();
app.InitializeDatabase();

app.Run();
