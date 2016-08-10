using DDD.Application.Service.Interface;
using DDD.DTO;
using DDD.Infra.Base;
using DDD.Infra.Session;
using DDD.Domain.Aggregates.UsuarioAgg;
using DDD.Infra.Validator;
using DDD.Domain.Enums;

namespace DDD.Application.Service
{
    public class UsuarioAppService : IUsuarioAppService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly ISessionHandler _sessionHandler;

        public UsuarioAppService(IRepositoryFactory repositoryFactory, ISessionHandler sessionHandler)
        {
            _repositoryFactory = repositoryFactory;
            _sessionHandler = sessionHandler;
        }

        #region IUsuarioAppService

        public void Add(UsuarioDTO usuarioDTO)
        {
            var usuario = new Usuario
            {
                Celular = usuarioDTO.Celular,
                Cpf = usuarioDTO.Cpf,
                Email = usuarioDTO.Email,
                Guid = usuarioDTO.Guid,
                IsAtivo = true,
                Nome = usuarioDTO.Nome,
                Perfil = ePerfil.AdministradorSistema,
                Telefone = usuarioDTO.Telefone,
                TenantId = 1,
            };

            usuario.SetSenhaCriptografada(usuarioDTO.Senha);
            usuario.SetSenhaConfirmaCriptografada(usuarioDTO.Senha);

            ValideUsuario(usuario);

            _repositoryFactory.UsuarioRepository.Add(usuario);
            _repositoryFactory.Commit();

        }

        #endregion

        #region Métodos Privados

        private void ValideUsuario(Usuario usuario)
        {
            var validationManager = new ValidationManager();

            var validator = EntityValidatorFactory.CreateValidator();
            validationManager.AddMessages(validator.GetInvalidMessages(usuario));

            validationManager.Validate();
        }

        #endregion

    }
}
