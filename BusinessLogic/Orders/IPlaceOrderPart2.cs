using BusinessLogic.GenericInterfaces;
using BusinessLogic.Orders.DTO;
using DataLayer.EfClasses;

namespace BusinessLogic.Orders;

public interface IPlaceOrderPart2 : IBusinessAction<Part1ToPart2Dto, Order>;
