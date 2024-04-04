using LibraryManagementSystem.SharedModels.Models;
using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Services.BussinessServices.IServices;
using LibraryManagementSystem.Services.BussinessServices.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using LibraryManagementSystem.Data.NewFolder;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LibraryDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IIssueBookRepository, IssueBookRepository>();
builder.Services.AddScoped<IReturnBookRepository, ReturnBookRepository>();
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

builder.Services.AddScoped<ILibraryManagementLogger, LibraryManagementLogger>();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IIssuedBookService, IssuedBookService>();
builder.Services.AddScoped<IReturnBookService, ReturnBookService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<LibraryDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Auth}/{action=Login}/{id?}");
});

AppDbInitializer.SeedUsersAndRolesAsync(app).Wait();

app.Run();

