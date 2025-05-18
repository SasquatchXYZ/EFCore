using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.GenericInterfaces;

public abstract class BusinessActionErrors
{
    private readonly List<ValidationResult> _errors = [];

    public IImmutableList<ValidationResult> Errors => _errors.ToImmutableList();

    public bool HasErrors => _errors.Count != 0;

    protected void AddError(string errorMessage, params string[] propertyNames) =>
        _errors.Add(new ValidationResult(errorMessage, propertyNames));
}
