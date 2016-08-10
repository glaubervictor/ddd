using DDD.Domain.Base;

namespace DDD.Domain.Aggregates.TenantAgg
{
    public interface ITenantRepository : IRepository<Tenant>
    {
    }
}
