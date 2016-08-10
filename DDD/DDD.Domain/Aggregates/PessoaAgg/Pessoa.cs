using DDD.Domain.Aggregates.TenantAgg;
using DDD.Domain.Base;

namespace DDD.Domain.Aggregates.PessoaAgg
{
    public abstract class Pessoa : Entity
    {
        #region Propriedades

        public int TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string Telefone { get; set; }

        #endregion
    }
}
