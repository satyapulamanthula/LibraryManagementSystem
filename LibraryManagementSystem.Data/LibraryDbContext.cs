using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class LibraryDbContext : IdentityDbContext<ApplicationUser> /*IdentityDbContext<ApplicationUser, AspNetRoles, string>*/
{
    public DbSet<Admin> Admins { get; set; }
    public DbSet<StudentEnrolment> StudentEnrolments { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<BooksCategory> BookCategories { get; set; }
    public DbSet<Issued> Issued { get; set; }


    //public DbSet<AspNetUsers> AspNetUsers { get; set; }
    //public DbSet<AspNetRoles> AspNetRoles { get; set; }
    //public DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Configure Book entity
        modelBuilder.Entity<Book>()
            .Property(b => b.Price)
            .HasColumnType("decimal(18, 2)");

        // Configure IssuedModel as a keyless entity
        modelBuilder.Entity<IssuedModel>().HasNoKey();

        //modelBuilder.Entity<AspNetUserRoles>().HasNoKey();

        // Additional configurations...

        base.OnModelCreating(modelBuilder);

        //base.OnModelCreating(modelBuilder);

        //// Configure Book entity
        //modelBuilder.Entity<Book>()
        //    .Property(b => b.Price)
        //    .HasColumnType("decimal(18, 2)");

        //// Configure composite primary key for AspNetUserRoles
        //modelBuilder.Entity<AspNetUserRoles>()
        //    .HasKey(ur => new { ur.UserId, ur.RoleId });
    }
}

