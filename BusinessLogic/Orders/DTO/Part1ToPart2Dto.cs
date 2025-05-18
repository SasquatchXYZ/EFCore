using System.Collections.Immutable;
using BusinessLogic.Orders.Models;
using DataLayer.EfClasses;

namespace BusinessLogic.Orders.DTO;

public class Part1ToPart2Dto
{
    public Part1ToPart2Dto(
        IImmutableList<OrderLineItem> lineItems,
        Order order)
    {
        LineItems = lineItems;
        Order = order;
    }

    public IImmutableList<OrderLineItem> LineItems { get; private set; }

    public Order Order { get; private set; }
}
