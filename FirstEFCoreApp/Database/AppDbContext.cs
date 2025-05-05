using FirstEFCoreApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstEFCoreApp.Database;

public class AppDbContext : DbContext
{
    private const string ConnectionString =
        @"Server=localhost\SQLEXPRESS;
            Database=master;
            Trusted_Connection=True";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionString);
    }

    public DbSet<Book> Books { get; set; }
}
