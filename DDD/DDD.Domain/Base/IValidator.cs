using System.Collections.Generic;

namespace DDD.Domain.Base
{
    public interface IValidator
    {
        IEnumerable<string[]> Validate();
    }
}
