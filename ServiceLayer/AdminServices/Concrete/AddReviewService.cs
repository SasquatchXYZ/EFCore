using DataLayer.EfClasses;
using DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.StatusGeneric;

namespace ServiceLayer.AdminServices.Concrete;

public class AddReviewService : IAddReviewService
{
    private readonly EfCoreContext _context;

    public AddReviewService(EfCoreContext context)
    {
        _context = context;
    }

    public string? BookTitle { get; private set; }

    public Review GetBlankReview(int id)
    {
        BookTitle = _context.Books
            .Where(book => book.BookId == id)
            .Select(book => book.Title)
            .Single();

        return new Review
        {
            BookId = id
        };
    }

    public Book AddReviewToBook(Review review)
    {
        var book = _context.Books
            .Include(book => book.Reviews)
            .Single(book => book.BookId == review.BookId);

        book.Reviews.Add(review);
        _context.SaveChanges();

        return book;
    }

    public IStatusGeneric AddReviewWithChecks(Review review)
    {
        var status = new StatusGenericHandler();

        if (review.NumStars < 0 || review.NumStars > 5)
            status.AddError("This must be between 0 and 5.", nameof(Review.NumStars));

        if (string.IsNullOrWhiteSpace(review.Comment))
            status.AddError("Please provide a comment with your review.", nameof(Review.Comment));

        if (!status.IsValid)
            return status;

        var book = _context.Books
            .Include(book => book.Reviews)
            .Single(book => book.BookId == review.BookId);

        book.Reviews.Add(review);
        _context.SaveChanges();
        return status;
    }
}
