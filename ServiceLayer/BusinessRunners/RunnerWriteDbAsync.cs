using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.GenericInterfaces;
using DataLayer.EfCode;

namespace ServiceLayer.BusinessRunners;

public class RunnerWriteDbAsync<TInput, TOutput>
{
    private readonly IBusinessActionAsync<TInput, TOutput?> _action;
    private readonly EfCoreContext _context;

    public RunnerWriteDbAsync(
        IBusinessActionAsync<TInput, TOutput?> action,
        EfCoreContext context)
    {
        _action = action;
        _context = context;
    }

    public IImmutableList<ValidationResult> Errors => _action.Errors;

    public bool HasErrors => _action.HasErrors;

    public async Task<TOutput?> RunActionAsync(TInput input)
    {
        var result = await _action.ActionAsync(input).ConfigureAwait(false);
        if (!HasErrors)
            await _context.SaveChangesAsync();

        return result;
    }
}
