using DDD.Domain.Aggregates.UsuarioAgg;
using System.Data.Entity.ModelConfiguration;

namespace DDD.Infra.UnitOfWork.Mapping
{
    public class UsuarioEntityConfig : EntityTypeConfiguration<Usuario>
    {
        public UsuarioEntityConfig()
        {
            HasKey(c => c.Id);
            Property(c => c.Id).HasColumnName("ID_CPES").IsRequired();
            Property(c => c.Perfil).HasColumnName("PERF_CUSU").IsRequired();
            Property(c => c.Senha).HasMaxLength(100).HasColumnName("SENHA_CUSU").IsRequired();
            Property(c => c.IsAtivo).HasColumnName("ATIV_CUSU");
            Property(c => c.Guid).HasColumnName("GUID_CUSU");
            Ignore(c => c.SenhaConfirma);

            ToTable("CADUSU");
        }
    }
}
