using DataLayer.EfClasses;

namespace ServiceLayer.BookServices.QueryObjects;

public static class BookListDtoSelect
{
    public static IQueryable<BookListDto> MapBookToDto(this IQueryable<Book> books)
    {
        return books.Select(book => new BookListDto
        {
            BookId = book.BookId,
            Title = book.Title,
            Price = book.Price,
            PublishedOn = book.PublishedOn,
            ActualPrice = book.Promotion == null
                ? book.Price
                : book.Promotion.NewPrice,
            PromotionPromotionalText = book.Promotion == null
                ? null
                : book.Promotion.PromotionalText,
            AuthorsOrdered = string.Join(", ",
                book.AuthorsLink
                    .OrderBy(bookAuthor => bookAuthor.Order)
                    .Select(bookAuthor => bookAuthor.Author.Name)),
            ReviewsCount = book.Reviews.Count,
            ReviewsAverageVotes = book.Reviews.Select(review => (double?) review.NumStars).Average(),
            TagStrings = book.Tags.Select(tag => tag.TagId).ToArray(),
        });
    }
}
