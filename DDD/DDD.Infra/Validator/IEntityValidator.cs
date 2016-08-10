using System.Collections.Generic;

namespace DDD.Infra.Validator
{
    public interface IEntityValidator
    {
        bool IsValid<TEntity>(TEntity item)
            where TEntity : class;

        IEnumerable<ValidationResult> GetInvalidMessages<TEntity>(TEntity item)
            where TEntity : class;
    }
}
