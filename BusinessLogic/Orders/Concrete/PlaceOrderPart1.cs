using BusinessDataAccess.Orders;
using BusinessLogic.GenericInterfaces;
using BusinessLogic.Orders.DTO;
using DataLayer.EfClasses;

namespace BusinessLogic.Orders.Concrete;

public class PlaceOrderPart1 : BusinessActionErrors, IPlaceOrderPart1
{
    private readonly IPlaceOrderDbAccess _dbAccess;

    public PlaceOrderPart1(IPlaceOrderDbAccess dbAccess)
    {
        _dbAccess = dbAccess;
    }

    public Part1ToPart2Dto? Action(PlaceOrderInDto dto)
    {
        if (!dto.AcceptTermsAndConditions)
            AddError("You must accept the Terms and Conditions to place and order.");

        if (!dto.LineItems.Any())
            AddError("No items in your basket.");

        var order = new Order
        {
            CustomerId = dto.UserId
        };

        if (!HasErrors)
            _dbAccess.Add(order);

        return new Part1ToPart2Dto(dto.LineItems, order);
    }
}
