using DreamsMade;
using DreamsMade.Crypto;
using DreamsMade.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var ConnectionStrings=builder.Configuration.GetConnectionString("Connection");
builder.Services.AddDbContext<Context>(opts =>
{
    opts.UseLazyLoadingProxies().UseSqlServer(ConnectionStrings);
});


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICrypto, Crypto>();
builder.Services.AddSingleton<UserResponse>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/Usuario/Index");
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});


var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();


app.Run();
