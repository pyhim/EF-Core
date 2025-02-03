using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EF_Core;

internal class ApplicationContext : DbContext
{
    public DbSet<Book> Books => Set<Book>();

    public ApplicationContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .SetBasePath(Directory.GetCurrentDirectory())
            .Build();

        optionsBuilder.UseSqlite(config.GetConnectionString("DefaultConnection"));
    }
}