using DataLayer.EfCode;
using ServiceLayer.BookServices.Enums;
using ServiceLayer.BookServices.Models;
using ServiceLayer.BookServices.QueryObjects;

namespace ServiceLayer.BookServices.Concrete;

public class BookFilterDropdownService
{
    private readonly EfCoreContext _context;

    public BookFilterDropdownService(EfCoreContext context)
    {
        _context = context;
    }

    public IEnumerable<DropdownTuple> GetFilterDropDownValues(BooksFilterBy filterBy)
    {
        switch (filterBy)
        {
            case BooksFilterBy.NoFilter:
                return new List<DropdownTuple>();
            case BooksFilterBy.ByVotes:
                return FormVotesDropDown();
            case BooksFilterBy.ByTags:
                return _context.Tags
                    .Select(tag => new DropdownTuple(tag.TagId, tag.TagId)).ToList();
            case BooksFilterBy.ByPublicationYear:
                var today = DateTime.UtcNow.Date;
                var result = _context.Books
                    .Where(book => book.PublishedOn <= today)
                    .Select(book => book.PublishedOn.Year)
                    .Distinct()
                    .OrderByDescending(year => year)
                    .Select(year => new DropdownTuple(year.ToString(), year.ToString())).ToList();

                var comingSoon = _context.Books
                    .Any(book => book.PublishedOn > today);

                if (comingSoon)
                    result.Insert(0, new DropdownTuple(
                        BookListDtoFilter.AllBooksNotPublishedString,
                        BookListDtoFilter.AllBooksNotPublishedString));

                return result;
            default:
                throw new ArgumentOutOfRangeException(nameof(filterBy), filterBy, null);
        }
    }

    private static IEnumerable<DropdownTuple> FormVotesDropDown() =>
    [
        new(Value: "4", Text: "4 Stars and Up"),
        new(Value: "3", Text: "3 Stars and Up"),
        new(Value: "2", Text: "2 Stars and Up"),
        new(Value: "1", Text: "1 Star and Up")
    ];
}
