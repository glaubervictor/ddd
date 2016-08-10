using DDD.Infra.Session;
using System;
using System.Web;
using DDD.Domain.Aggregates.UsuarioAgg;
using DDD.Domain.Enums;
using DDD.Infra.Base;
using DDD.Domain.Base;
using Newtonsoft.Json;

namespace DDD.Web.Security
{
    public class SessionHandler : ISessionHandler
    {
        #region Métodos

        internal static void Autenticar(string email, string senha, IUsuarioRepository usuarioRepository)
        {
            #region Validação

            if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(senha))
            {
                throw new AppException("Usuário e senha não informados.");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new AppException("Usuário não informado.");
            }

            if (string.IsNullOrWhiteSpace(senha))
            {
                throw new AppException("Senha não informada.");
            }

            #endregion

            #region Obter Usuário

            var usuario = usuarioRepository.Get(email);

            if (usuario == null)
            {
                throw new AppException("Usuário e/ou senha inválidos.");
            }

            if (!usuario.IsAtivo)
            {
                throw new AppException("Usuário não esta ativo.");
            }

            if (!Function.Cryptografa(senha).Equals(usuario.Senha))
            {
                throw new AppException("Usuário e/ou senha inválidos.");
            }

            var usuarioDados = new Usuario
            {
                Id       = usuario.Id,
                Guid     = usuario.Guid,
                TenantId = usuario.TenantId,
                Email    = usuario.Email,
                IsAtivo  = usuario.IsAtivo,
                Perfil   = usuario.Perfil,
                Nome     = usuario.Nome,
                Cpf      = usuario.Cpf
            };

            var userName = JsonConvert.SerializeObject(usuarioDados);
            var userData = usuarioDados.Perfil.Value.ToString();

            FormsAuthenticationService.SignIn(1, userName, false, userData);

            #endregion
        }

        public static void Desautenticar()
        {
            FormsAuthenticationService.SignOut();
        }

        public void ValidaAutenticacao()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                throw new Exception("Usuário não autenticado.");
        }


        #endregion

        #region ISessionHandler

        public int GetUsuarioId()
        {
            if (UserStore.Usuario != null)
            {
                return UserStore.Usuario.Id;
            }
            else
            {
                return 0;
            }
        }

        public Usuario GetUsuario()
        {
            if (UserStore.Usuario != null)
            {
                return UserStore.Usuario;
            }
            else
            {
                return null;
            }

        }

        public int GetTenantId()
        {
            if (UserStore.Usuario != null)
            {
                return UserStore.Usuario.TenantId;
            }
            else
            {
                return 0;
            }
        }

        public ePerfil? GetPerfil()
        {
            if (UserStore.Usuario != null)
            {
                return UserStore.Usuario.Perfil;
            }
            else
            {
                return null;
            }
        }

        public bool IsAutenticado()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }
        
        #endregion

        private class UserStore
        {
            #region Propriedades

            public static Usuario Usuario
            {
                get
                {
                    Usuario usuario = null;

                    if (HttpContext.Current.Request.IsAuthenticated)
                    {
                        usuario = (Usuario)JsonConvert.DeserializeObject(HttpContext.Current.User.Identity.Name);
                    }

                    return usuario;
                }
            }
            
            #endregion
        }
    }
}