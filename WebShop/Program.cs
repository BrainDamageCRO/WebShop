using Microsoft.EntityFrameworkCore;

using WebShop.DataAccess.Data;
using WebShop.DataAccess.Repository;
using WebShop.DataAccess.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add DbContext as a service with connection string from appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Repository service
// builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


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
app.UseAuthorization();

// Default route - if nothing is defined go to Home/Index/..
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Runs the project
app.Run();

// update-database -> create a database without any migration at beginning 
// add-migration -> create a migration 
// update-database -> after add-migration needed to actually update the db