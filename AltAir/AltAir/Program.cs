using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AltAir.Data;
using Microsoft.AspNetCore.Identity;
using AltAir.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AltAirContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("AltAirContext") ?? throw new InvalidOperationException("Connection string 'AltAirContext' not found.")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>().AddEntityFrameworkStores<AltAirContext>();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
