using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.GenericInterfaces;
using DataLayer.EfCode;

namespace ServiceLayer.BusinessRunners;

public class RunnerWriteDb<TInput, TOutput>
{
    private readonly IBusinessAction<TInput, TOutput?> _action;
    private readonly EfCoreContext _context;

    public RunnerWriteDb(
        IBusinessAction<TInput, TOutput?> action,
        EfCoreContext context)
    {
        _action = action;
        _context = context;
    }

    public IImmutableList<ValidationResult> Errors => _action.Errors;

    public bool HasErrors => _action.HasErrors;

    public TOutput? RunAction(TInput input)
    {
        var result = _action.Action(input);
        if (!HasErrors)
            _context.SaveChanges();

        return result;
    }
}
