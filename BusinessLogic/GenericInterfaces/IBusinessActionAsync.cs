using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.GenericInterfaces;

public interface IBusinessActionAsync<in TInput, TOutput>
{

    IImmutableList<ValidationResult> Errors { get; }

    bool HasErrors { get; }

    Task<TOutput?> ActionAsync(TInput dto);
}
