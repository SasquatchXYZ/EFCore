using BusinessLogic.GenericInterfaces;
using BusinessLogic.Orders.DTO;

namespace BusinessLogic.Orders;

public interface IPlaceOrderPart1 : IBusinessAction<PlaceOrderInDto, Part1ToPart2Dto?>;
