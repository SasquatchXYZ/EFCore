using Microsoft.AspNetCore.Http;

namespace ServiceLayer.CheckoutServices.Concrete;

public abstract class CookieTemplate
{
    private readonly string _cookieName;
    private readonly IRequestCookieCollection _requestCookieCollection;
    private readonly IResponseCookies? _responseCookies;

    protected CookieTemplate(
        string cookieName,
        IRequestCookieCollection requestCookieCollection,
        IResponseCookies? responseCookies = null)
    {
        if (requestCookieCollection is null)
            throw new ArgumentNullException(nameof(requestCookieCollection));

        _cookieName = cookieName;
        _requestCookieCollection = requestCookieCollection;
        _responseCookies = responseCookies;
    }

    protected virtual int ExpiresInThisManyDays
    {
        get => 0;
    }

    public void AddOrUpdateCookie(string value)
    {
        if (_responseCookies is null)
            throw new NullReferenceException("You must supply a IResponseCookies value if you want to use this command.");

        var options = new CookieOptions();
        if (ExpiresInThisManyDays > 0)
            options.Expires = DateTime.Now.AddDays(ExpiresInThisManyDays);

        _responseCookies.Append(_cookieName, value, options);
    }

    public bool Exists() => _requestCookieCollection[_cookieName] is not null;

    public string? GetValue()
    {
        var cookie = _requestCookieCollection[_cookieName];
        return string.IsNullOrEmpty(cookie)
            ? null
            : cookie;
    }

    public void DeleteCookie()
    {
        if (_responseCookies is null)
            throw new NullReferenceException("You must supply a IResponseCookies value if you want to use this command.");

        if (!Exists()) return;
        var options = new CookieOptions
        {
            Expires = DateTime.Now.AddYears(-1)
        };

        _responseCookies.Append(_cookieName, string.Empty, options);
    }
}
