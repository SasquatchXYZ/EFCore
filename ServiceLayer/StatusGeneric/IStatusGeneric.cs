namespace ServiceLayer.StatusGeneric;

public interface IStatusGeneric
{
    IReadOnlyList<ErrorGeneric> Errors { get; }

    bool IsValid { get; }

    bool HasErrors { get; }

    string Message { get; set; }

    IStatusGeneric CombineStatuses(IStatusGeneric status);

    string GetAllErrors(string? separator = null);
}
