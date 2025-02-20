using Microsoft.EntityFrameworkCore;

namespace Homework2;

public class ApplicationContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    
    public ApplicationContext()
    {
        Database.EnsureCreated();
        Console.WriteLine("Database connection established!");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=catalog.sqlite3");
    }

    ~ApplicationContext()
    {
        Console.WriteLine("Database is closing...");    
    }
}