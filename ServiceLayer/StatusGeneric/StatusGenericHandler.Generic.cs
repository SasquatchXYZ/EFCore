using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.StatusGeneric;

public class StatusGenericHandler<T> : StatusGenericHandler, IStatusGeneric<T>
{
    private T? _result;

    public T? Result => IsValid ? _result : default;

    public StatusGenericHandler<T> SetResult(T result)
    {
        _result = result;
        return this;
    }

    public new IStatusGeneric<T> AddError(string? errorMessage, params string[] propertyNames)
    {
        if (errorMessage is null) throw new ArgumentNullException(nameof(errorMessage));
        _errors.Add(new ErrorGeneric(Header, new ValidationResult(errorMessage, propertyNames)));

        return this;
    }
}
