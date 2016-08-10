using DDD.Domain.Base;

namespace DDD.Domain.Aggregates.UsuarioAgg
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Usuario Get(string email);
    }
}
