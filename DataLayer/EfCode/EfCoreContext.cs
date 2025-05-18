using DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.EfCode;

public class EfCoreContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<PriceOffer> PriceOffers { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Order> Orders { get; set; }

    public EfCoreContext(DbContextOptions<EfCoreContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookAuthor>()
            .HasKey(bookAuthor => new
            {
                bookAuthor.BookId,
                bookAuthor.AuthorId
            });

        // modelBuilder.Entity<LineItem>()
        //     .HasOne(p => p.ChosenBook)
        //     .WithMany()
        //     .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Book>()
            .HasQueryFilter(p => !p.SoftDeleted);

        // modelBuilder.Entity<Order>()
        // .HasQueryFilter(x => x.CustomerId == _userId);
    }
}
