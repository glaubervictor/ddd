using DDD.Domain.Aggregates.TenantAgg;
using System.Data.Entity.ModelConfiguration;

namespace DDD.Infra.UnitOfWork.Mapping
{
    public class TenantEntityConfig : EntityTypeConfiguration<Tenant>
    {
        public TenantEntityConfig()
        {
            HasKey(c => c.Id);
            Property(c => c.Id).HasColumnName("ID_CTEN").IsRequired();
            Property(c => c.Descricao).HasColumnName("DESC_CTEN").HasMaxLength(100).IsRequired();
            ToTable("CADTEN");
        }
    }
}
