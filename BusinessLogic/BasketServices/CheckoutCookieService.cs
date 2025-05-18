using System.Collections.Immutable;
using System.Text;
using BusinessLogic.Orders.Models;

namespace BusinessLogic.BasketServices;

public class CheckoutCookieService
{
    private readonly List<OrderLineItem> _lineItems;

    public CheckoutCookieService(string? cookieContent)
    {
        _lineItems = [];
        DecodeCookieString(cookieContent);
    }

    public Guid UserId { get; private set; }
    public ImmutableList<OrderLineItem> LineItems => _lineItems.ToImmutableList();

    public void AddLineItem(OrderLineItem newItem) => _lineItems.Add(newItem);

    public void DeleteLineItem(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex > _lineItems.Count)
            throw new InvalidOperationException("Could not find that item");

        _lineItems.RemoveAt(itemIndex);
    }

    public void ClearAllLineItems() => _lineItems.Clear();

    public string EncodeForCookie()
    {
        var sb = new StringBuilder();
        sb.Append(UserId.ToString("N"));
        foreach (var lineItem in _lineItems)
        {
            sb.Append($",{lineItem.BookId},{lineItem.NumBooks}");
        }

        return sb.ToString();
    }

    private void DecodeCookieString(string? cookieContent)
    {
        if (cookieContent is null)
        {
            UserId = Guid.NewGuid();
            return;
        }

        var parts = cookieContent.Split(',');
        UserId = Guid.Parse(parts[0]);
        for (var i = 0; i < (parts.Length - 1) / 2; i++)
        {
            _lineItems.Add(new OrderLineItem
            {
                BookId = int.Parse(parts[i * 2 + 1]),
                NumBooks = short.Parse(parts[i * 2 + 2]),
            });
        }
    }
}
