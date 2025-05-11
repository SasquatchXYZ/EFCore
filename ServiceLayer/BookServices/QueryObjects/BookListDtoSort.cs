using ServiceLayer.BookServices.Enums;

namespace ServiceLayer.BookServices.QueryObjects;

public static class BookListDtoSort
{
    public static IQueryable<BookListDto> OrderBooksBy(this IQueryable<BookListDto> books, OrderByOptions orderByOptions)
    {
        return orderByOptions switch
        {
            OrderByOptions.SimpleOrder => books.OrderByDescending(book => book.BookId),
            OrderByOptions.ByVotes => books.OrderByDescending(book => book.ReviewsAverageVotes),
            OrderByOptions.ByPublicationDate => books.OrderByDescending(book => book.PublishedOn),
            OrderByOptions.ByPriceLowestFirst => books.OrderBy(book => book.ActualPrice),
            OrderByOptions.ByPriceHighestFirst => books.OrderByDescending(book => book.ActualPrice),
            _ => throw new ArgumentOutOfRangeException(nameof(orderByOptions), orderByOptions, null)
        };
    }
}
