using EmployeeGraphQLDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeGraphQLDemo.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Employee> Employees => Set<Employee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>().Property(e => e.Salary).HasPrecision(18, 2);

        // Seed data: inserted by the migration.
        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, Name = "Alice Johnson", Email = "alice@example.com", Department = "IT", Designation = "Developer", Salary = 60000 },
            new Employee { Id = 2, Name = "Bob Smith", Email = "bob@example.com", Department = "HR", Designation = "HR Manager", Salary = 55000 },
            new Employee { Id = 3, Name = "Carol White", Email = "carol@example.com", Department = "IT", Designation = "Tech Lead", Salary = 85000 },
            new Employee { Id = 4, Name = "David Brown", Email = "david@example.com", Department = "Finance", Designation = "Accountant", Salary = 50000 }
        );
    }
}
