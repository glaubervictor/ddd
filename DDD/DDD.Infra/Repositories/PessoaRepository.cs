using DDD.Domain.Aggregates.PessoaAgg;
using DDD.Infra.Base;
using DDD.Infra.UnitOfWork;

namespace DDD.Infra.Repositories
{
    public class PessoaRepository : Repository<Pessoa>, IPessoaRepository
    {
        public PessoaRepository(MainBcUnitOfWork uow)
            : base(uow, uow.Pessoa)
        {

        }
    }
}
