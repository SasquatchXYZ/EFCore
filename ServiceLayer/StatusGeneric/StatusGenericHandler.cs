using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.StatusGeneric;

public class StatusGenericHandler : IStatusGenericHandler
{
    public const string DefaultSuccessMessage = "Success";
    protected readonly List<ErrorGeneric> _errors = [];
    private string _successMessage = DefaultSuccessMessage;

    public StatusGenericHandler(string header = "")
    {
        Header = header;
    }

    public string Header { get; set; }

    public IReadOnlyList<ErrorGeneric> Errors => _errors.AsReadOnly();

    public bool IsValid => _errors.Count == 0;

    public bool HasErrors => _errors.Count > 0;

    public string Message
    {
        get => IsValid
            ? _successMessage
            : $"Failed with {_errors.Count} error{(_errors.Count == 1 ? string.Empty : "s")}";
        set => _successMessage = value;
    }

    public IStatusGeneric CombineStatuses(IStatusGeneric status)
    {
        if (!status.IsValid)
        {
            _errors.AddRange(string.IsNullOrEmpty(Header)
                ? status.Errors
                : status.Errors.Select(error => new ErrorGeneric(Header, error)));
        }

        if (IsValid && status.Message != DefaultSuccessMessage)
            Message = status.Message;

        return this;
    }

    public string GetAllErrors(string? separator = null)
    {
        separator = separator ?? Environment.NewLine;
        return _errors.Any()
            ? string.Join(separator, Errors)
            : "No Errors";
    }

    public IStatusGeneric AddError(string? errorMessage, params string[] propertyNames)
    {
        if (errorMessage is null) throw new ArgumentNullException(nameof(errorMessage));
        _errors.Add(new ErrorGeneric(Header, new ValidationResult(errorMessage, propertyNames)));
        return this;
    }

    public IStatusGeneric AddError(Exception ex, string? errorMessage, params string[] propertyNames)
    {
        if (errorMessage is null) throw new ArgumentNullException(nameof(errorMessage));
        var errorGeneric = new ErrorGeneric(Header, new ValidationResult(errorMessage, propertyNames));
        errorGeneric.CopyExceptionToDebugData(ex);
        _errors.Add(errorGeneric);
        return this;
    }

    public void AddValidationResult(ValidationResult validationResult)
    {
        _errors.Add(new ErrorGeneric(Header, validationResult));
    }

    public void AddValidationResults(IEnumerable<ValidationResult> validationResults)
    {
        _errors.AddRange(validationResults.Select(result => new ErrorGeneric(Header, result)));
    }
}
