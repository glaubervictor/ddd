using AutoMapper;
using AutoMapper.Configuration.Conventions;
using AutoMapper.Mappers;

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
            Mapper.Initialize(cfg => 
            {
                cfg.CreateMissingTypeMaps = true;
                
                //Conventions
                cfg.AddMemberConfiguration().AddMember<NameSplitMember>().AddName<PrePostfixName>(_ => _.AddStrings(p => p.Postfixes, "DTO"));
                cfg.AddConditionalObjectMapper().Where((s, d) => s.Name == d.Name + "DTO");

                //Manually Maps
                //cfg.CreateMap<TenantDTO, Tenant>();
            });

            Mapper.AssertConfigurationIsValid();
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
