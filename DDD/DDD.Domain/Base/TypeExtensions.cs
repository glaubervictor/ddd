using System;

namespace DDD.Domain.Base
{
    /// <summary>
    /// Extension methods for the Type class
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Return true if the type is a System.Nullable wrapper of a value type
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>True if the type is a System.Nullable wrapper</returns>
        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
    }
}