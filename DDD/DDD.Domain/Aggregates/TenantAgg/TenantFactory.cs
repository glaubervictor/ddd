namespace DDD.Domain.Aggregates.TenantAgg
{
    public static class TenantFactory
    {
        public static Tenant Create(string descricao)
        {
            var tenant = new Tenant();
            tenant.Descricao = descricao;

            return tenant;
        }
    }
}
