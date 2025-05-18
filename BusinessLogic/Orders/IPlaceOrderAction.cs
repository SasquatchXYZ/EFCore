using BusinessLogic.GenericInterfaces;
using BusinessLogic.Orders.DTO;
using DataLayer.EfClasses;

namespace BusinessLogic.Orders;

public interface IPlaceOrderAction : IBusinessAction<PlaceOrderInDto, Order>;
