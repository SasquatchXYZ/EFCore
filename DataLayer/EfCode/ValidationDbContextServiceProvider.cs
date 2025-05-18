using Microsoft.EntityFrameworkCore;

namespace DataLayer.EfCode;

public class ValidationDbContextServiceProvider : IServiceProvider
{
    private readonly DbContext _currentContext;

    public ValidationDbContextServiceProvider(DbContext currentContext)
    {
        _currentContext = currentContext;
    }

    public object? GetService(Type serviceType)
    {
        if (serviceType == typeof(DbContext))
        {
            return _currentContext;
        }

        return null;
    }
}
