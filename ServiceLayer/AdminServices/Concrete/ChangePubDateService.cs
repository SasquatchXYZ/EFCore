using DataLayer.EfClasses;
using DataLayer.EfCode;

namespace ServiceLayer.AdminServices.Concrete;

public class ChangePubDateService : IChangePubDateService
{
    private readonly EfCoreContext _context;

    public ChangePubDateService(EfCoreContext context)
    {
        _context = context;
    }

    public ChangePubDateDto GetOriginal(int id)
    {
        return _context.Books
            .Select(book => new ChangePubDateDto
            {
                BookId = book.BookId,
                Title = book.Title,
                PublishedOn = book.PublishedOn
            })
            .Single(book => book.BookId == id);
    }

    public Book UpdateBook(ChangePubDateDto dto)
    {
        var book = _context.Books.SingleOrDefault(book => book.BookId == dto.BookId);
        if (book is null)
            throw new ArgumentException("Book not found");

        book.PublishedOn = dto.PublishedOn;
        _context.SaveChanges();
        return book;
    }
}
