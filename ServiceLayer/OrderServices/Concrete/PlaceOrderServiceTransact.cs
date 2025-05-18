using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using BusinessDataAccess.Orders;
using BusinessLogic.BasketServices;
using BusinessLogic.Orders.Concrete;
using BusinessLogic.Orders.DTO;
using DataLayer.EfClasses;
using DataLayer.EfCode;
using Microsoft.AspNetCore.Http;
using ServiceLayer.BusinessRunners;
using ServiceLayer.CheckoutServices.Concrete;

namespace ServiceLayer.OrderServices.Concrete;

public class PlaceOrderServiceTransact
{
    private readonly BasketCookie _basketCookie;
    private readonly RunnerTransact2WriteDb<PlaceOrderInDto, Part1ToPart2Dto, Order> _runner;

    public PlaceOrderServiceTransact(
        IRequestCookieCollection requestCookieCollection,
        IResponseCookies responseCookies,
        EfCoreContext context)
    {
        _basketCookie = new BasketCookie(requestCookieCollection, responseCookies);
        _runner = new RunnerTransact2WriteDb<PlaceOrderInDto, Part1ToPart2Dto, Order>(
            context,
            new PlaceOrderPart1(
                new PlaceOrderDbAccess(context)),
            new PlaceOrderPart2(
                new PlaceOrderDbAccess(context))
            );
    }

    public IImmutableList<ValidationResult>? Errors => _runner.Errors;

    public int PlaceOrder(bool acceptTermsAndConditions)
    {
        var checkoutService = new CheckoutCookieService(_basketCookie.GetValue());

        var order = _runner.RunAction(
            new PlaceOrderInDto(acceptTermsAndConditions,
                checkoutService.UserId,
                checkoutService.LineItems));

        if (_runner.HasErrors) return 0;

        checkoutService.ClearAllLineItems();
        _basketCookie.AddOrUpdateCookie(
            checkoutService.EncodeForCookie());

        return order!.OrderId;
    }
}
