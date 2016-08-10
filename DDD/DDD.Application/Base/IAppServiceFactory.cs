using DDD.Application.Service.Interface;

namespace DDD.Application.Base
{
    public interface IAppServiceFactory
    {
        ITenantAppService TenantAppService { get; }
        IUsuarioAppService UsuarioAppService { get; }
        void Dispose();   
    }
}
