using DDD.Domain.Aggregates.PessoaAgg;
using DDD.Domain.Aggregates.TenantAgg;
using DDD.Domain.Aggregates.UsuarioAgg;

namespace DDD.Infra.Base
{
    public interface IRepositoryFactory
    {
        ITenantRepository TenantRepository { get; }
        IPessoaRepository PessoaRepository { get; }
        IUsuarioRepository UsuarioRepository { get; }
        void Commit();
        void Dispose();
    }
}
