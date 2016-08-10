using DDD.Domain.Aggregates.TenantAgg;
using DDD.Infra.Base;
using DDD.Infra.UnitOfWork;

namespace DDD.Infra.Repositories
{
    public class TenantRepository : Repository<Tenant>, ITenantRepository
    {
        public TenantRepository(MainBcUnitOfWork uow)
            : base(uow, uow.Tenant)
        {

        }
    }
}
