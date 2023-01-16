using System.Configuration;
using Balogh_David_Project.Data;
using Balogh_David_Project.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Balogh_David_Project.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<LaptopContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("LaptopConnection")));

builder.Services.AddDbContext<Balogh_David_ProjectIdentityDbContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("Balogh_David_ProjectIdentityDbContextConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<Balogh_David_ProjectIdentityDbContext>();

builder.Services.AddSignalR();
builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddAuthorization(opts => {
    opts.AddPolicy("OnlySales", policy => {
        policy.RequireClaim("Department", "Sales");
    });
});

builder.Services.AddAuthorization(opts => {
    opts.AddPolicy("SalesManager", policy => {
        policy.RequireRole("Manager");
        policy.RequireClaim("Department", "Sales");
    });
});

builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    // DbInitializer.Initialize(services);
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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/Chat");
app.MapRazorPages();

app.Run();

