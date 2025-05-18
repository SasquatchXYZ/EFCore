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

public class PlaceOrderService
{
    private readonly BasketCookie _basketCookie;
    private readonly RunnerWriteDb<PlaceOrderInDto, Order?> _runner;
    public IImmutableList<ValidationResult> Errors => _runner.Errors;

    public PlaceOrderService(
        IRequestCookieCollection requestCookieCollection,
        IResponseCookies responseCookies,
        EfCoreContext context)
    {
        _basketCookie = new BasketCookie(requestCookieCollection, responseCookies);
        _runner = new RunnerWriteDb<PlaceOrderInDto, Order?>(
            new PlaceOrderAction(
                new PlaceOrderDbAccess(context)),
            context);
    }

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
