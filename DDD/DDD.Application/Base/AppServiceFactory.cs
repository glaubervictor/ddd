using DDD.Application.Service.Interface;
using DDD.Infra.Base;
using DDD.Application.Service;
using DDD.Infra.Session;

namespace DDD.Application.Base
{
    public class AppServiceFactory : IAppServiceFactory
    {
        private readonly ISessionHandler _sessionHandler;
        private readonly IRepositoryFactory _repositoryFactory;

        private ITenantAppService _tenantAppService;
        private IUsuarioAppService _usuarioAppService;

        public AppServiceFactory(IRepositoryFactory repositoryFactory, ISessionHandler sessionHandler)
        {
            _repositoryFactory = repositoryFactory;
            _sessionHandler = sessionHandler;
        }

        public ITenantAppService TenantAppService
        {
            get
            {
                return _tenantAppService ?? (_tenantAppService = new TenantAppService(_repositoryFactory, _sessionHandler));
            }
        }

        public IUsuarioAppService UsuarioAppService
        {
            get
            {
                return _usuarioAppService ?? (_usuarioAppService = new UsuarioAppService(_repositoryFactory, _sessionHandler));
            }
        }

        public void Dispose()
        {
            _repositoryFactory.Dispose();

            if (_tenantAppService != null)
                _tenantAppService.Dispose();
        }
    }
}
