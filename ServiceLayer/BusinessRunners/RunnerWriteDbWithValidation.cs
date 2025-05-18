using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.GenericInterfaces;
using DataLayer.EfCode;

namespace ServiceLayer.BusinessRunners;

public class RunnerWriteDbWithValidation<TInput, TOutput>
{

    private readonly IBusinessAction<TInput, TOutput?> _action;
    private readonly EfCoreContext _context;

    public RunnerWriteDbWithValidation(
        IBusinessAction<TInput, TOutput?> action,
        EfCoreContext context)
    {
        _action = action;
        _context = context;
    }

    public IImmutableList<ValidationResult>? Errors { get; private set; }

    public bool HasErrors => Errors?.Any() ?? false;

    public TOutput? RunAction(TInput input)
    {
        var result = _action.Action(input);
        Errors = _action.Errors;
        if (!HasErrors)
        {
            Errors = _context.SaveChangesWithValidation();
        }

        return result;
    }
}
