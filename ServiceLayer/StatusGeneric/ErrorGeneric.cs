using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServiceLayer.StatusGeneric;

public struct ErrorGeneric
{
    public const string HeaderSeparator = ">";

    public ErrorGeneric(string header, ValidationResult error) : this()
    {
        Header = header ?? throw new ArgumentNullException(nameof(header));
        ErrorResult = error ?? throw new ArgumentNullException(nameof(error));
    }

    internal ErrorGeneric(string? prefix, ErrorGeneric existingError)
    {
        Header = string.IsNullOrEmpty(prefix)
            ? existingError.Header
            : string.IsNullOrEmpty(existingError.Header)
                ? prefix
                : prefix + HeaderSeparator + existingError.Header;

        ErrorResult = existingError.ErrorResult;
        DebugData = existingError.DebugData;
    }

    public string? Header { get; private set; }
    public ValidationResult ErrorResult { get; private set; }
    public string DebugData { get; private set; }

    internal void CopyExceptionToDebugData(Exception ex)
    {
        var sb = new StringBuilder();
        sb.AppendLine(ex.Message);
        sb.Append("StackTrace:");
        sb.AppendLine(ex.StackTrace);
        foreach (DictionaryEntry entry in ex.Data)
        {
            sb.AppendLine($"Data: {entry.Key}\t{entry.Value}");
        }

        DebugData = sb.ToString();
    }

    public override string ToString()
    {
        var start = string.IsNullOrEmpty(Header)
            ? string.Empty
            : $"{Header}: ";

        return $"{start}{ErrorResult}";
    }
}
