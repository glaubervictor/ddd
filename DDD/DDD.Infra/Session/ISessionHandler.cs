using DDD.Domain.Aggregates.UsuarioAgg;
using DDD.Domain.Enums;

namespace DDD.Infra.Session
{
    public interface ISessionHandler
    {
        int GetUsuarioId();

        Usuario GetUsuario();

        int GetTenantId();

        ePerfil? GetPerfil();

        bool IsAutenticado();
        
    }
}
