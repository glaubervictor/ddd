using DDD.Domain.Base;
using System.Collections.Generic;

namespace DDD.Domain.Aggregates.TenantAgg
{
    public class Tenant : Entity
    {
        public string Descricao { get; set; }

        public override IEnumerable<string[]> Validate()
        {
            if (string.IsNullOrWhiteSpace(Descricao))
            {
                yield return Validation.Add<Tenant>(c => c.Descricao, ValidationMessages.Required());
            }
        }
    }
}
