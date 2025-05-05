using FirstEFCoreApp.Database;
using Microsoft.EntityFrameworkCore;

namespace FirstEFCoreApp;

public static class Commands
{
    public static void ListAll()
    {
        using (var db = new AppDbContext())
        {
            // `AsNoTracking` indicates that this access is read-only
            foreach (var book in db.Books.AsNoTracking()
                         .Include(book => book.Author))
            {
                var webUrl = book.Author.WebUrl ?? "- no web url given -";
                Console.WriteLine($"{book.Title} by {book.Author.Name}");
                Console.WriteLine($"     Published on {book.PublishedOn:dd-MMM-yyyy}. {webUrl}");
            }
        }
    }
}
