using LibraryManagementSystem.Data.NewFolder;
using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.Repository.Repositories;
using LibraryManagementSystem.Services.BussinessServices.IServices;
using LibraryManagementSystem.Services.BussinessServices.Services;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure Database
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repositories
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IIssueBookRepository, IssueBookRepository>();
builder.Services.AddScoped<IReturnBookRepository, ReturnBookRepository>();
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

// Register Services
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IIssuedBookService, IssuedBookService>();
builder.Services.AddScoped<IReturnBookService, ReturnBookService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<LibraryDbContext>()
    .AddDefaultTokenProviders();

// Configure Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
});

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Configure routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

// Seed initial database data
AppDbInitializer.SeedUsersAndRolesAsync(app).Wait();

// Run the application
app.Run();



//Ignore the below 

//var builder = WebApplication.CreateBuilder(args);

//// Configure Serilog from appsettings.json
////builder.Host.UseSerilog((context, services, configuration) =>
//   // configuration.ReadFrom.Configuration(context.Configuration));

//// Add services to the container.
//builder.Services.AddControllersWithViews();
//builder.Services.AddDbContext<LibraryDbContext>(options => options.UseSqlServer(
//    builder.Configuration.GetConnectionString("DefaultConnection")  
//));

//// Register repositories and services
//builder.Services.AddScoped<IBookRepository, BookRepository>();
//builder.Services.AddScoped<IStudentRepository, StudentRepository>();
//builder.Services.AddScoped<IIssueBookRepository, IssueBookRepository>();
//builder.Services.AddScoped<IReturnBookRepository, ReturnBookRepository>();
//builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

//builder.Services.AddScoped<ILibraryManagementLogger, LibraryManagementLogger>();

//builder.Services.AddScoped<IBookService, BookService>();
//builder.Services.AddScoped<IStudentService, StudentService>();
//builder.Services.AddScoped<IIssuedBookService, IssuedBookService>();
//builder.Services.AddScoped<IReturnBookService, ReturnBookService>();
//builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

//builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//    .AddEntityFrameworkStores<LibraryDbContext>()
//    .AddDefaultTokenProviders();

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//});

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Auth}/{action=Login}/{id?}");

//app.MapControllerRoute(
//    name: "customRouting",
//    pattern: "{controller}/{action}");

//app.MapControllerRoute(
//    name: "customRouting",
//    pattern: "{controller}/{action}/{id}");

//app.UseEndpoints(endpoints =>
//{
//    _ = endpoints.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Auth}/{action=Login}/{id?}");
//});

//// Seed initial database data
//AppDbInitializer.SeedUsersAndRolesAsync(app).Wait();

//app.Run();

//// Ensure Serilog closes and flushes log entries on application exit
////Log.CloseAndFlush();


////using LibraryManagementSystem.Data.NewFolder;
////using LibraryManagementSystem.Repository.IRepositories;
////using LibraryManagementSystem.Repository.Repositories;
////using LibraryManagementSystem.Services.BussinessServices.IServices;
////using LibraryManagementSystem.Services.BussinessServices.Services;
////using LibraryManagementSystem.SharedModels.Models;
////using Microsoft.AspNetCore.Authentication.Cookies;
////using Microsoft.AspNetCore.Identity;
////using Microsoft.EntityFrameworkCore;

////var builder = WebApplication.CreateBuilder(args);

////// Add services to the container.
////builder.Services.AddControllersWithViews();
////builder.Services.AddDbContext<LibraryDbContext>(options => options.UseSqlServer(
////    builder.Configuration.GetConnectionString("DefaultConnection")
////    ));

////builder.Services.AddScoped<IBookRepository, BookRepository>();
////builder.Services.AddScoped<IStudentRepository, StudentRepository>();
////builder.Services.AddScoped<IIssueBookRepository, IssueBookRepository>();
////builder.Services.AddScoped<IReturnBookRepository, ReturnBookRepository>();
////builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

////builder.Services.AddScoped<ILibraryManagementLogger, LibraryManagementLogger>();

////builder.Services.AddScoped<IBookService, BookService>();
////builder.Services.AddScoped<IStudentService, StudentService>();
////builder.Services.AddScoped<IIssuedBookService, IssuedBookService>();
////builder.Services.AddScoped<IReturnBookService, ReturnBookService>();
////builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

////builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
////    .AddEntityFrameworkStores<LibraryDbContext>()
////    .AddDefaultTokenProviders();

////builder.Services.AddAuthentication(options =>
////{
////    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
////});
////var app = builder.Build();

////// Configure the HTTP request pipeline.
////if (!app.Environment.IsDevelopment())
////{
////    app.UseExceptionHandler("/Home/Error");
////    app.UseHsts();
////}

////app.UseHttpsRedirection();
////app.UseStaticFiles();

////app.UseRouting();

////app.UseAuthentication();
////app.UseAuthorization();

////app.MapControllerRoute(
////    name: "default",
////    pattern: "{controller=Auth}/{action=Login}/{id?}");

////app.UseEndpoints(endpoints =>
////{
////    endpoints.MapControllerRoute(
////        name: "default",
////        pattern: "{controller=Auth}/{action=Login}/{id?}");
////});

////AppDbInitializer.SeedUsersAndRolesAsync(app).Wait();

////app.Run();

