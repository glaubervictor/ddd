using DDD.Domain.Aggregates.PessoaAgg;
using DDD.Domain.Aggregates.TenantAgg;
using DDD.Domain.Aggregates.UsuarioAgg;
using DDD.Infra.UnitOfWork;
using DDD.Infra.Repositories;
using System;

namespace DDD.Infra.Base
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly MainBcUnitOfWork _uow;
        private IPessoaRepository _pessoaRepository;
        private ITenantRepository _tenantRepository;
        private IUsuarioRepository _usuarioRepository;

        public RepositoryFactory(MainBcUnitOfWork uow)
        {
            _uow = uow;
        }
        
        public IPessoaRepository PessoaRepository
        {
            get
            {
                return _pessoaRepository ?? (_pessoaRepository = new PessoaRepository(_uow));
            }
        }

        public ITenantRepository TenantRepository
        {
            get
            {
                return _tenantRepository ?? (_tenantRepository = new TenantRepository(_uow));
            }
        }

        public IUsuarioRepository UsuarioRepository
        {
            get
            {
                return _usuarioRepository ?? (_usuarioRepository = new UsuarioRepository(_uow));
            }
        }

        public void Commit()
        {
            _uow.Commit();
        }

        public void Dispose()
        {
            if (_tenantRepository != null)
                _tenantRepository.Dispose();

            if (_pessoaRepository != null)
                _pessoaRepository.Dispose();
            
            if (_usuarioRepository != null)
                _usuarioRepository.Dispose();
        }

        
    }
}
