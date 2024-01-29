using Microsoft.EntityFrameworkCore;
using Biblioteka.Context;
using Biblioteka.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using JsonApiDotNetCore.Configuration;
using System;
using Biblioteka.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<BibContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCS"));
});

builder.Services.AddDbContext<BibContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCS"));
});

// Configuring ASP Identity
builder.Services.AddDefaultIdentity<BibUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;

})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<BibContext>()
.AddDefaultTokenProviders()
.AddErrorDescriber<BibErrorDescriber>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddJsonApi<BibContext>(options =>
{
    options.UseRelativeLinks = true;
    options.AllowUnknownQueryStringParameters = true;
    options.SerializerOptions.WriteIndented = true;
});

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
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//        name: "api",
//        pattern: "api/{controller}/{action}"); // Dodaj akcj� tutaj
//});

app.UseAuthorization();

app.UseJsonApi();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

//Tworzenie r�l i przyk�aowych kont
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "Employee", "Reader" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<BibUser>>();

    string email = "admin@admin.com";
    string password = "admin123";
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new BibUser();

        user.name = "Admin";
        user.surname = "Admin";
        user.UserName = email;
        user.Email = email;
        user.EmailConfirmed = true;

        await userManager.CreateAsync(user, password);

        await userManager.AddToRoleAsync(user, "Admin");
    }
    email = "employee@employee.com";
    password = "employee123";
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new BibUser();

        user.name = "Employee";
        user.surname = "Employee";
        user.UserName = email;
        user.Email = email;
        user.EmailConfirmed = true;

        await userManager.CreateAsync(user, password);

        await userManager.AddToRoleAsync(user, "Employee");
    }   
}

app.Run();
