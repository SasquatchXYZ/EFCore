using BusinessDataAccess.Orders;
using BusinessLogic.GenericInterfaces;
using BusinessLogic.Orders.DTO;
using BusinessLogic.Orders.Models;
using DataLayer.EfClasses;

namespace BusinessLogic.Orders.Concrete;

public class PlaceOrderAction :
    BusinessActionErrors,
    IBusinessAction<PlaceOrderInDto, Order?>
{
    private readonly IPlaceOrderDbAccess _dbAccess;

    public PlaceOrderAction(IPlaceOrderDbAccess dbAccess)
    {
        _dbAccess = dbAccess;
    }

    public Order? Action(PlaceOrderInDto dto)
    {
        if (!dto.AcceptTermsAndConditions)
        {
            AddError("You must accept the Terms and Conditions to place an order.");
            return null;
        }

        if (!dto.LineItems.Any())
        {
            AddError("No items in your basket.");
            return null;
        }

        var booksDictionary =
            _dbAccess.FindBooksByIdsWithPriceOffers(dto.LineItems.Select(book => book.BookId));

        var order = new Order
        {
            CustomerId = dto.UserId,
            LineItems = FormLineItemsWithErrorChecking(dto.LineItems, booksDictionary),
        };

        if (!HasErrors)
            _dbAccess.Add(order);

        return HasErrors
            ? null
            : order;
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
                throw new InvalidOperationException($"An order failed because book, id = {lineItem.BookId} was missing.");

            var bookPrice = book.Promotion?.NewPrice ?? book.Price;

            if (bookPrice <= 0)
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
