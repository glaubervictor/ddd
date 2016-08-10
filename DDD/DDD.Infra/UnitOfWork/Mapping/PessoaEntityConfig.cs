using DDD.Domain.Aggregates.PessoaAgg;
using System.Data.Entity.ModelConfiguration;

namespace DDD.Infra.UnitOfWork.Mapping
{
    public class PessoaEntityConfig : EntityTypeConfiguration<Pessoa>
    {
        public PessoaEntityConfig()
        {
            HasKey(c => c.Id);
            Property(c => c.Id).HasColumnName("ID_CPES").IsRequired();
            Property(c => c.TenantId).HasColumnName("ID_CTEN").IsRequired();
            Property(c => c.Celular).HasColumnName("CEL_CPES").HasMaxLength(14).IsRequired();
            Property(c => c.Cpf).HasColumnName("CPF_CPES").HasMaxLength(11).IsRequired();
            Property(c => c.Email).HasColumnName("EMAIL_CPES").HasMaxLength(100).IsRequired();
            Property(c => c.Nome).HasColumnName("NOME_CPES").HasMaxLength(100).IsRequired();
            Property(c => c.Telefone).HasColumnName("TEL_CPES").HasMaxLength(14).IsRequired();

            HasRequired(c => c.Tenant).WithMany().HasForeignKey(c => c.TenantId);
            ToTable("CADPES");
        }
    }
}
