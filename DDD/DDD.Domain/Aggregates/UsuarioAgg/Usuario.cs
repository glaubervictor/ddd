using DDD.Domain.Aggregates.PessoaAgg;
using DDD.Domain.Enums;
using System.Collections.Generic;
using System;
using DDD.Domain.Base;

namespace DDD.Domain.Aggregates.UsuarioAgg
{
    public class Usuario : Pessoa
    {
        #region Propriedades

        public ePerfil? Perfil { get; set; }
        public string Senha { get; private set; }
        public string SenhaConfirma { get; private set; }
        public string Guid { get; set; }
        public bool IsAtivo { get; set; }

        #endregion

        #region Métodos Públicos

        public void SetSenhaCriptografada(string senha)
        {
            Senha = Function.Cryptografa(senha);
        }

        public void SetSenha(string senha)
        {
            Senha = senha;
        }

        public void SetSenhaConfirmaCriptografada(string senhaConfirma)
        {
            SenhaConfirma = Function.Cryptografa(senhaConfirma);
        }

        public void SetSenhaConfirma(string senhaConfirma)
        {
            SenhaConfirma = senhaConfirma;
        }


        #endregion

        public override IEnumerable<string[]> Validate()
        {
            throw new NotImplementedException();
        }
    }
}
