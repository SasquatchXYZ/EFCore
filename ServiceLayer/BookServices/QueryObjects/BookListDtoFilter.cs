using ServiceLayer.BookServices.Enums;

namespace ServiceLayer.BookServices.QueryObjects;

public static class BookListDtoFilter
{
    public const string AllBooksNotPublishedString = "Coming Soon";

    public static IQueryable<BookListDto> FilterBooksBy(this IQueryable<BookListDto> books, BooksFilterBy filterBy, string? filterValue)
    {
        if (string.IsNullOrEmpty(filterValue))
            return books;

        switch (filterBy)
        {
            case BooksFilterBy.NoFilter:
                return books;
            case BooksFilterBy.ByVotes:
                var filterVote = int.Parse(filterValue);
                return books.Where(book => book.ReviewsAverageVotes > filterVote);
            case BooksFilterBy.ByTags:
                return books.Where(book => book.TagStrings.Any(tag => tag == filterValue));
            case BooksFilterBy.ByPublicationYear:
                if (filterValue == AllBooksNotPublishedString)
                    return books.Where(book => book.PublishedOn > DateTime.UtcNow);

                var filterYear = int.Parse(filterValue);
                return books.Where(book => book.PublishedOn.Year == filterYear && book.PublishedOn <= DateTime.UtcNow);
            default:
                throw new ArgumentOutOfRangeException(nameof(filterBy), filterBy, null);
        }
    }
}
