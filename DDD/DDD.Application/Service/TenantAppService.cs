using System;
using DDD.Application.Service.Interface;
using DDD.Infra.Base;
using System.Linq;
using DDD.DTO;
using DDD.Domain.Aggregates.TenantAgg;
using DDD.Infra.Session;
using DDD.Infra.Validator;

namespace DDD.Application.Service
{
    public class TenantAppService : ITenantAppService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly ISessionHandler _sessionHandler;

        public TenantAppService(IRepositoryFactory repositoryFactory, ISessionHandler sessionHandler)
        {
            _repositoryFactory = repositoryFactory;
            _sessionHandler = sessionHandler;
        }

        #region ITenantAppService

        public void Add(TenantDTO tenantDTO)
        {
            var tenant = new Tenant
            {
                Descricao = tenantDTO.Descricao
            };

            ValideTenant(tenant);
            
            _repositoryFactory.TenantRepository.Add(tenant);
            _repositoryFactory.Commit();

            
        }

        public void GetAll()
        {
            var lista = _repositoryFactory.TenantRepository.GetAll().ToList();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }



        #endregion

        #region Métodos Privados

        private void ValideTenant(Tenant tenant)
        {
            var validationManager = new ValidationManager();

            var validator = EntityValidatorFactory.CreateValidator();
            validationManager.AddMessages(validator.GetInvalidMessages(tenant));
            
            validationManager.Validate();
        }

        #endregion
    }
}
