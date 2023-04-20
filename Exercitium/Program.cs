using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Exercitium.Data;
using Microsoft.AspNetCore.Identity;
using Exercitium.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ExercitiumContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ExercitiumContext") ?? throw new InvalidOperationException("Connection string 'ExercitiumContext' not found.")));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ExercitiumContext>();

builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    await Seed.SeedUsersAndRolesAsync(app);
    //Seed.SeedData(app);
}

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
