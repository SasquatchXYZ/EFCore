using Microsoft.AspNetCore.Http;

namespace ServiceLayer.CheckoutServices.Concrete;

public class BasketCookie : CookieTemplate
{
    public const string BasketCookieName = "efcore-basket";

    public BasketCookie(
        IRequestCookieCollection requestCookieCollection,
        IResponseCookies? responseCookies = null)
        : base(BasketCookieName,
            requestCookieCollection,
            responseCookies)
    {
    }

    protected override int ExpiresInThisManyDays => 200;
}
