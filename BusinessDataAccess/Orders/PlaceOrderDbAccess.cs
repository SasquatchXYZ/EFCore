using DataLayer.EfClasses;
using DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;

namespace BusinessDataAccess.Orders;

public interface IPlaceOrderDbAccess
{
    IDictionary<int, Book> FindBooksByIdsWithPriceOffers(
        IEnumerable<int> bookIds);

    void Add(Order newOrder);
}

public class PlaceOrderDbAccess : IPlaceOrderDbAccess
{
    private readonly EfCoreContext _context;

    public PlaceOrderDbAccess(EfCoreContext context)
    {
        _context = context;
    }

    public IDictionary<int, Book> FindBooksByIdsWithPriceOffers(
        IEnumerable<int> bookIds)
    {
        return _context.Books
            .Where(book => bookIds.Contains(book.BookId))
            .Include(book => book.Promotion)
            .ToDictionary(key => key.BookId);
    }

    public void Add(Order newOrder)
    {
        _context.Orders.Add(newOrder);
    }
}
