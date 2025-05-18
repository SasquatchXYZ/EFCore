using BusinessDataAccess.Orders;
using BusinessLogic.GenericInterfaces;
using BusinessLogic.Orders.DTO;
using BusinessLogic.Orders.Models;
using DataLayer.EfClasses;

namespace BusinessLogic.Orders.Concrete;

public class PlaceOrderPart2 : BusinessActionErrors, IPlaceOrderPart2
{
    private readonly IPlaceOrderDbAccess _dbAccess;

    public PlaceOrderPart2(IPlaceOrderDbAccess dbAccess)
    {
        _dbAccess = dbAccess;
    }

    public Order? Action(Part1ToPart2Dto dto)
    {
        var booksDictionary =
            _dbAccess.FindBooksByIdsWithPriceOffers(dto.LineItems.Select(lineItem => lineItem.BookId));

        dto.Order.LineItems = FormLineItemsWithErrorChecking(dto.LineItems, booksDictionary);

        return HasErrors ? null : dto.Order;
    }

    private List<LineItem> FormLineItemsWithErrorChecking(
        IEnumerable<OrderLineItem> lineItems,
        IDictionary<int, Book> booksDictionary)
    {
        var result = new List<LineItem>();
        var i = 1;

        foreach (var lineItem in lineItems)
        {
            if (!booksDictionary.TryGetValue(lineItem.BookId, out var book))
                throw new InvalidOperationException(
                    $"Could not find the {i} book you wanted to order.  Please remove that book and try again."
                );

            var bookPrice = book.Promotion?.NewPrice ?? book.Price;

            if (book.PublishedOn > DateTime.UtcNow)
            {
                AddError($"Sorry, the book '{book.Title}' is not yet in print.");
            }
            else if (bookPrice <= 0)
            {
                AddError($"Sorry, the book '{book.Title}' is not for sale.");
            }
            else
            {
                result.Add(new LineItem
                {
                    BookPrice = bookPrice,
                    ChosenBook = book,
                    LineNum = (byte) i++,
                    NumBooks = lineItem.NumBooks
                });
            }
        }

        return result;
    }
}
