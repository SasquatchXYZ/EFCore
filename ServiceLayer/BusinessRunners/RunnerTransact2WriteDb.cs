using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.GenericInterfaces;
using DataLayer.EfCode;

namespace ServiceLayer.BusinessRunners;

public class RunnerTransact2WriteDb<TInput, TPass, TOutput>
    where TPass : class
    where TOutput : class
{

    private readonly EfCoreContext _context;
    private readonly IBusinessAction<TInput, TPass?> _actionPartOne;
    private readonly IBusinessAction<TPass?, TOutput?> _actionPartTwo;

    public RunnerTransact2WriteDb(
        EfCoreContext context,
        IBusinessAction<TInput, TPass?> actionPartOne,
        IBusinessAction<TPass?, TOutput?> actionPartTwo)
    {
        _context = context;
        _actionPartOne = actionPartOne;
        _actionPartTwo = actionPartTwo;
    }

    public IImmutableList<ValidationResult>? Errors { get; private set; }

    public bool HasErrors => Errors?.Any() ?? false;

    public TOutput? RunAction(TInput input)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            var partialResult = RunPart(_actionPartOne, input);
            if (HasErrors) return null;

            var result = RunPart(_actionPartTwo, partialResult);

            if (!HasErrors)
            {
                transaction.Commit();
            }

            return result;
        }
    }

    private TPartOut? RunPart<TPartIn, TPartOut>(
        IBusinessAction<TPartIn, TPartOut?> businessAction,
        TPartIn input)
        where TPartOut : class
    {
        var result = businessAction.Action(input);
        Errors = businessAction.Errors;
        if (!HasErrors)
        {
            _context.SaveChanges();
        }

        return result;
    }
}
