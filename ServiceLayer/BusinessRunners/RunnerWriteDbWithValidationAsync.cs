using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.GenericInterfaces;
using DataLayer.EfCode;

namespace ServiceLayer.BusinessRunners;

public class RunnerWriteDbWithValidationAsync<TInput, TOutput>
{
    private readonly IBusinessActionAsync<TInput, TOutput?> _action;
    private readonly EfCoreContext _context;

    public RunnerWriteDbWithValidationAsync(
        IBusinessActionAsync<TInput, TOutput?> action,
        EfCoreContext context)
    {
        _action = action;
        _context = context;
    }

    public IImmutableList<ValidationResult>? Errors { get; private set; }

    public bool HasErrors => Errors?.Any() ?? false;

    public async Task<TOutput?> RunActionAsync(TInput input)
    {
        var result = await _action.ActionAsync(input).ConfigureAwait(false);
        Errors = _action.Errors;
        if (!HasErrors)
        {
            Errors = await _context
                .SaveChangesWithValidationAsync()
                .ConfigureAwait(false);
        }

        return result;
    }
}
