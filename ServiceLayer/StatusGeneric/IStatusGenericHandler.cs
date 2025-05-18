using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.StatusGeneric;

public interface IStatusGenericHandler : IStatusGeneric
{
    IStatusGeneric AddError(string errorMessage, params string[] propertyNames);

    IStatusGeneric AddError(Exception ex, string errorMessage, params string[] propertyNames);

    void AddValidationResult(ValidationResult validationResult);

    void AddValidationResults(IEnumerable<ValidationResult> validationResults);
}
