using MySql.Data.Entity;
using DDD.Domain.Aggregates.TenantAgg;
using DDD.Infra.Base;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Data.Entity.Validation;
using System.Collections.Generic;
using DDD.Domain.Aggregates.PessoaAgg;
using DDD.Domain.Aggregates.UsuarioAgg;
using DDD.Infra.UnitOfWork.Mapping;
using DDD.Infra.Session;

namespace DDD.Infra.UnitOfWork
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class MainBcUnitOfWork : DbContext, IQueryableUnitOfWork
    {
        ISessionHandler _sessionHandler;

        #region Construtor

        public MainBcUnitOfWork() { }

        public MainBcUnitOfWork(ISessionHandler sessionHandler)
        {
            _sessionHandler = sessionHandler;
            var tenantId    = sessionHandler.GetTenantId();

            if (tenantId > 0)
            {
                Tenant  = new FilteredDbSet<Tenant>(this);
                Pessoa  = new FilteredDbSet<Pessoa>(this, c => c.TenantId == tenantId, c => c.TenantId = tenantId);
                Usuario = new FilteredDbSet<Usuario>(this, c => c.TenantId == tenantId, c => c.TenantId = tenantId);
            }
            else
            {
                Tenant  = CreateSet<Tenant>();
                Pessoa  = CreateSet<Pessoa>();
                Usuario = CreateSet<Usuario>();
            }
        }

        #endregion

        #region Membros de IDbSet

        public IDbSet<Tenant> Tenant { get; set; }
        public IDbSet<Pessoa> Pessoa { get; set; }
        public IDbSet<Usuario> Usuario { get; set; }

        #endregion

        #region IQueryableUnitOfWork

        public DbSet<TEntity> CreateSet<TEntity>()
            where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public void Attach<TEntity>(TEntity item)
            where TEntity : class
        {
            //attach and set as unchanged
            base.Entry<TEntity>(item).State = EntityState.Unchanged;
        }

        public void SetModified<TEntity>(TEntity item)
            where TEntity : class
        {
            //this operation also attach item in object state manager
            base.Entry<TEntity>(item).State = EntityState.Modified;
        }

        public void DeleteObject<TEntity>(TEntity item)
            where TEntity : class
        {
            base.Entry<TEntity>(item).State = EntityState.Deleted;
        }

        public void ApplyCurrentValues<TEntity>(TEntity original, TEntity current)
            where TEntity : class
        {
            //if it is not attached, attach original and set current values
            base.Entry<TEntity>(original).CurrentValues.SetValues(current);
        }

        public void Commit()
        {
            IEnumerable<DbEntityValidationResult> errors = new List<DbEntityValidationResult>();

            if (Configuration.ValidateOnSaveEnabled)
            {
                errors = GetValidationErrors();
            }

            if (!errors.Any())
            {
                base.SaveChanges();
            }
            else
            {
                //throw new DatabaseValidationErrors(errors);
            }
        }

        public void CommitAndRefreshChanges()
        {
            var saveFailed = false;

            do
            {
                try
                {
                    base.SaveChanges();

                    saveFailed = false;

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    ex.Entries.ToList().ForEach(entry => { entry.OriginalValues.SetValues(entry.GetDatabaseValues()); });

                }
            }
            while (saveFailed);
        }

        public void RollbackChanges()
        {
            ChangeTracker.Entries().ToList().ForEach(entry => entry.State = EntityState.Unchanged);
        }


        #endregion

        #region Override DbContext

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Remove unused conventions
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            // Add entity configurations in a structured way using 'TypeConfiguration’ classes
            modelBuilder.Configurations.Add(new TenantEntityConfig());
            modelBuilder.Configurations.Add(new PessoaEntityConfig());
            modelBuilder.Configurations.Add(new UsuarioEntityConfig());

            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}
