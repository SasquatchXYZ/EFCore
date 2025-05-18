using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.EfCode;

public static class DbContextValidationHelper
{
    public static async Task<ImmutableList<ValidationResult>> SaveChangesWithValidationAsync(
        this DbContext context)
    {
        var result = context.ExecuteValidation();
        if (!result.IsEmpty) return result;

        context.ChangeTracker.AutoDetectChangesEnabled = false;
        try
        {
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
        finally
        {
            context.ChangeTracker.AutoDetectChangesEnabled = true;
        }

        return result;
    }

    public static ImmutableList<ValidationResult> SaveChangesWithValidation(
        this DbContext context)
    {
        var result = context.ExecuteValidation();
        if (!result.IsEmpty) return result;

        context.ChangeTracker.AutoDetectChangesEnabled = false;
        try
        {
            context.SaveChanges();
        }
        finally
        {
            context.ChangeTracker.AutoDetectChangesEnabled = true;
        }

        return result;
    }

    private static ImmutableList<ValidationResult> ExecuteValidation(
        this DbContext context)
    {
        var result = new List<ValidationResult>();
        foreach (var entry in context.ChangeTracker.Entries()
                     .Where(entry => entry.State is EntityState.Added or EntityState.Modified))
        {
            var entity = entry.Entity;
            var validationProvider = new ValidationDbContextServiceProvider(context);
            var validationContext = new ValidationContext(entity, validationProvider, null);
            var entityErrors = new List<ValidationResult>();
            if (!Validator.TryValidateObject(entity, validationContext, entityErrors, true))
            {
                result.AddRange(entityErrors);
            }
        }

        return result.ToImmutableList();
    }
}
