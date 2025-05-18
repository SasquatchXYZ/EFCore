using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.GenericInterfaces;

public interface IBusinessAction<TInput, TOutput>
{
    IImmutableList<ValidationResult> Errors { get; }

    bool HasErrors { get; }

    TOutput? Action(TInput dto);
}
