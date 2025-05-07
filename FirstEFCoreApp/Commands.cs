using FirstEFCoreApp.Database;
using FirstEFCoreApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

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

    public static void ChangeWebUrl()
    {
        Console.Write("New Quantum Networking WebUrl > ");
        var newWebUrl = Console.ReadLine();

        using (var db = new AppDbContext())
        {
            var singleBook = db.Books
                .Include(book => book.Author)
                .Single(book => book.Title == "Quantum Networking");

            singleBook.Author.WebUrl = newWebUrl;
            db.SaveChanges();
            Console.WriteLine("... SaveChanges called.");
        }

        ListAll();
    }

    public static void ListAllWithLogs()
    {
        var logs = new List<string>();
        using (var db = new AppDbContext())
        {
            var serviceProvider = db.GetInfrastructure();
            var loggerFactory = (ILoggerFactory?) serviceProvider.GetService(typeof(ILoggerFactory));
            loggerFactory?.AddProvider(new MyLoggerProvider(logs));

            foreach (var entity in
                     db.Books.AsNoTracking()
                         .Include(book => book.Author))
            {
                var webUrl = entity.Author.WebUrl is null
                    ? "- no web url given -"
                    : entity.Author.WebUrl;

                Console.WriteLine($"{entity.Title} by {entity.Author.Name}");
                Console.WriteLine($"     Published on {entity.PublishedOn:dd-MMM-yyyy}. {webUrl}");
            }
        }

        Console.WriteLine("--------- Logs ---------------------");
        foreach (var log in logs)
        {
            Console.WriteLine(log);
        }
    }

    public static void ChangeWebUrlWithLogs()
    {
        var logs = new List<string>();
        Console.Write("New Quantum Networking WebUrl > ");
        var newWebUrl = Console.ReadLine();

        using (var db = new AppDbContext())
        {
            var serviceProvider = db.GetInfrastructure();
            var loggerFactory = (ILoggerFactory?) serviceProvider.GetService(typeof(ILoggerFactory));
            loggerFactory?.AddProvider(new MyLoggerProvider(logs));

            var singleBook = db.Books
                .Include(book => book.Author)
                .Single(book => book.Title == "Quantum Networking");

            singleBook.Author.WebUrl = newWebUrl;
            db.SaveChanges();
            Console.WriteLine("... SaveChanges called.");
        }

        Console.WriteLine("--------- Logs ---------------------");
        foreach (var log in logs)
        {
            Console.WriteLine(log);
        }
    }

    public static bool WipeCreateSeed(bool onlyIfNoDatabase)
    {
        using (var db = new AppDbContext())
        {
            if (onlyIfNoDatabase && ((db.GetService<IDatabaseCreator>() as RelationalDatabaseCreator)?.Exists() ?? false))
                return false;

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            if (!db.Books.Any())
            {
                WriteTestData(db);
                Console.WriteLine("Seeded database");
            }
        }

        return true;
    }

    public static void WriteTestData(this AppDbContext db)
    {
        var martinFowler = new Author
        {
            Name = "Martin Fowler",
            WebUrl = "https://martinfowler.com/",
        };

        var books = new List<Book>
        {
            new()
            {
                Title = "Refactoring",
                Description = "Improving the design of existing code",
                PublishedOn = new DateTime(1999, 7, 8),
                Author = martinFowler,
            },
            new()
            {
                Title = "Patterns of Enterprise Application Architecture",
                Description = "Written in direct response to the stiff challenges",
                PublishedOn = new DateTime(2002, 11, 15),
                Author = martinFowler,
            },
            new()
            {
                Title = "Domain-Driven Design",
                Description = "Linking business needs to software design",
                PublishedOn = new DateTime(2003, 8, 30),
                Author = new Author
                {
                    Name = "Eric Evans",
                    WebUrl = "https://domainlanguage.com/"
                },
            },
            new()
            {
                Title = "Quantum Networking",
                Description = "Entangled quantum networking provides faster-than-light data communication",
                PublishedOn = new DateTime(2057, 1, 1),
                Author = new Author
                {
                    Name = "Future Person"
                },
            },
        };

        db.Books.AddRange(books);
        db.SaveChanges();
    }
}
