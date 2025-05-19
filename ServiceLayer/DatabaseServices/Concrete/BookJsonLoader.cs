using DataLayer.EfClasses;
using Newtonsoft.Json;

namespace ServiceLayer.DatabaseServices.Concrete;

public static class BookJsonLoader
{
    public static IEnumerable<Book> LoadBooks(string fileDir, string fileSearchString)
    {
        var filePath = GetJsonFilePath(fileDir, fileSearchString);
        var jsonDecoded = JsonConvert.DeserializeObject<ICollection<BookInfoJson>>(File.ReadAllText(filePath));

        if (jsonDecoded is null)
            throw new NullReferenceException($"Unable to deserialize the file {filePath}.");

        var authorsDictionary = new Dictionary<string, Author>();
        var tagsDictionary = new Dictionary<string, Tag>();
        foreach (var bookInfoJson in jsonDecoded)
        {
            foreach (var author in bookInfoJson.authors)
            {
                if (!authorsDictionary.ContainsKey(author))
                    authorsDictionary[author] = new Author
                    {
                        Name = author
                    };
            }

            foreach (var category in bookInfoJson.categories)
            {
                if (!tagsDictionary.ContainsKey(category))
                    tagsDictionary[category] = new Tag
                    {
                        TagId = category
                    };
            }
        }

        return jsonDecoded.Select(json => CreateBookWithRefs(json, authorsDictionary, tagsDictionary));
    }

    private static Book CreateBookWithRefs(
        BookInfoJson bookInfoJson,
        Dictionary<string, Author> authorsDictionary,
        Dictionary<string, Tag> tagsDictionary)
    {
        var book = new Book
        {
            Title = bookInfoJson.title,
            Description = bookInfoJson.description,
            PublishedOn = DecodePublishDate(bookInfoJson.publishedDate),
            Publisher = bookInfoJson.publisher,
            Price = (decimal) (bookInfoJson.saleInfoListPriceAmount ?? -1),
            ImageUrl = bookInfoJson.imageLinksThumbnail
        };

        byte i = 0;
        book.AuthorsLink = new List<BookAuthor>();
        foreach (var author in bookInfoJson.authors)
        {
            book.AuthorsLink.Add(new BookAuthor
            {
                Book = book,
                Author = authorsDictionary[author],
                Order = i++
            });
        }

        book.Tags = new List<Tag>();
        foreach (var category in bookInfoJson.categories)
        {
            book.Tags.Add(tagsDictionary[category]);
        }

        if (bookInfoJson.averageRating is not null &&
            bookInfoJson.ratingsCount is not null)
            book.Reviews =
                CalculateReviewsToMatch(
                    (double) bookInfoJson.averageRating,
                    (int) bookInfoJson.ratingsCount);

        return book;
    }

    internal static IList<Review> CalculateReviewsToMatch(double averageRating, int ratingsCount)
    {
        var reviews = new List<Review>();
        var currentAvg = averageRating;
        for (var i = 0; i < ratingsCount; i++)
        {
            reviews.Add(new Review
            {
                VoterName = "Anonymous",
                NumStars = (int) (currentAvg > averageRating
                    ? Math.Truncate(averageRating)
                    : Math.Ceiling(averageRating))
            });

            currentAvg = reviews.Average(x => x.NumStars);
        }

        return reviews;
    }

    private static DateTime DecodePublishDate(string publishedDate)
    {
        var split = publishedDate.Split('-');
        switch (split.Length)
        {
            case 1:
                return new DateTime(int.Parse(split[0]), 1, 1);
            case 2:
                return new DateTime(int.Parse(split[0]), int.Parse(split[1]), 1);
            case 3:
                return new DateTime(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
        }

        throw new InvalidOperationException($"The json publishedDate failed to decode: string was {publishedDate}");
    }

    private static string GetJsonFilePath(string fileDir, string searchPattern)
    {
        var fileList = Directory.GetFiles(fileDir, searchPattern);

        if (fileList.Length == 0)
            throw new FileNotFoundException($"Cound not find a file with the search name of {searchPattern} in directory {fileDir}");

        return fileList.ToList().OrderBy(x => x).Last();
    }
}
