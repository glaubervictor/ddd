using AutoMapper;
using AutoMapper.Mappers;
using DDD.Domain.Aggregates.TenantAgg;
using DDD.Domain.Base;
using DDD.DTO;
using System.Linq;

namespace DDD.Infra.Adapter
{
    public class AutomapperTypeAdapterFactory : Profile, ITypeAdapterFactory
    {
        #region Constructor

        /// <summary>
        /// Create a new Automapper type adapter factory
        /// </summary>
        public AutomapperTypeAdapterFactory()
        {
            var entityAssembly  = typeof(Tenant).Assembly;
            var modelAssembly   = typeof(TenantDTO).Assembly;
            var modelNamespace  = modelAssembly.GetTypes().FirstOrDefault().Namespace;

            foreach (var entity in entityAssembly.GetTypes().Where(a => a.BaseType == typeof(Entity)))
            {
                var model = modelAssembly.GetType($"{modelNamespace}.{entity.Name}DTO");

                if (model != null)
                {
                    Mapper.CreateMap(entity, model);
                }
            }
        }

        #endregion

        #region ITypeAdapterFactory Members

        public ITypeAdapter Create()
        {
            return new AutomapperTypeAdapter();
        }

        #endregion
    }
}
