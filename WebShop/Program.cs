using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess.Data;
using WebShop.DataAccess.Repository;
using WebShop.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using WebShop.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext as a service with connection string from appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// This needs to be added because of Identity 
builder.Services.AddRazorPages();

// builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

// Always need to be added after AddIdentity - won't work other way around
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});


// Add Repository service
// builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Use https instead http
app.UseHttpsRedirection();

// Adds all wwwroot files
app.UseStaticFiles();

// Adds routing
// https://localhost:55555/Category/Index 
// https://localhost:55555/Category
// https://localhost:55555/Category/Edit/3
// https://localhost:55555/Product/Details/3
app.UseRouting();

// This is something we will work on later
app.UseAuthentication();
app.UseAuthorization();
// This needs to be added because of Identity 
app.MapRazorPages();
// Default route - if nothing is defined go to Home/Index/..
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

// Runs the project
app.Run();

// update-database -> create a database without any migration at beginning 
// add-migration -> create a migration 
// update-database -> after add-migration needed to actually update the db