using DDD.Domain.Aggregates.UsuarioAgg;
using DDD.Infra.Base;
using DDD.Infra.UnitOfWork;
using System.Linq;

namespace DDD.Infra.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(MainBcUnitOfWork uow)
            : base(uow, uow.Usuario)
        {

        }

        public Usuario Get(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                var uow = UnitOfWork as MainBcUnitOfWork;
                var set = uow.Usuario;

                return set.Where(c => c.Email == email).SingleOrDefault();
            }
            else
                return null;
        }
    }
}
