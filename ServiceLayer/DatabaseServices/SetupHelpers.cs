using DataLayer.EfCode;
using ServiceLayer.DatabaseServices.Concrete;

namespace ServiceLayer.DatabaseServices;

public static class SetupHelpers
{
    private const string SeedDataSearchName = "Apress books*.json";
    public const string SeedFileSubDirectory = "seedData";

    public static async Task<int> SeedDatabaseIfNoBooksAsync(
        this EfCoreContext context,
        string dataDirectory)
    {
        var numBooks = context.Books.Count();
        if (numBooks == 0)
        {
            var books = BookJsonLoader.LoadBooks(Path.Combine(dataDirectory, SeedFileSubDirectory), SeedDataSearchName).ToList();
            context.Books.AddRange(books);
            await context.SaveChangesAsync();

            context.Books.Add(SpecialBook.CreateSpecialBook());
            await context.SaveChangesAsync();
            numBooks = books.Count + 1;
        }

        return numBooks;
    }
}
