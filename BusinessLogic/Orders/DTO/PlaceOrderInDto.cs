using System.Collections.Immutable;
using BusinessLogic.Orders.Models;

namespace BusinessLogic.Orders.DTO;

public class PlaceOrderInDto
{
    public PlaceOrderInDto(
        bool acceptTermsAndConditions,
        Guid userId,
        IImmutableList<OrderLineItem> lineItems)
    {
        AcceptTermsAndConditions = acceptTermsAndConditions;
        UserId = userId;
        LineItems = lineItems;
    }

    public bool AcceptTermsAndConditions { get; private set; }
    public Guid UserId { get; private set; }
    public IImmutableList<OrderLineItem> LineItems { get; private set; }
}
