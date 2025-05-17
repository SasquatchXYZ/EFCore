using DataLayer.EfClasses;

namespace ServiceLayer.AdminServices;

public interface IAddReviewService
{
    string? BookTitle { get; }
    Review GetBlankReview(int id);
    Book AddReviewToBook(Review review);
}
