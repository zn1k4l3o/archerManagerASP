using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ArcherManager.Web.Data;
using ArcherManager.DAL;
using ArcherManager.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ArcherManagerDbContext>(options =>
    options.UseSqlServer(connectionString, opt => opt.MigrationsAssembly("ArcherManager.DAL")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    });


builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ArcherManagerDbContext>();
builder.Services.AddControllersWithViews();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "natjecanja",
    pattern: "natjecanja/{action=Index}/{id?}",
    defaults: new { controller = "Competition" });
app.MapControllerRoute(
    name: "strelicari",
    pattern: "strelicari/{action=Index}/{id?}",
    defaults: new { controller = "Archer" });
app.MapControllerRoute(
    name: "klubovi",
    pattern: "klubovi/{action=Index}/{id?}",
    defaults: new { controller = "Club" });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
