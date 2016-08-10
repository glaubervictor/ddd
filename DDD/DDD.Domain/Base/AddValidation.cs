using System;
using System.Linq.Expressions;

namespace DDD.Domain.Base
{
    /// <summary>
    /// Link a property to a validation result.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>

    public static class Validation
    {
        public static string[] Add<TEntity>(Expression<Func<TEntity, object>> property, string errorMessage)
        {
            return new string[] { errorMessage, LambdaUtilities.GetExpressionText(property) };
        }
    }
}
