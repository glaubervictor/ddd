using Autofac;
using Autofac.Integration.Mvc;
using DDD.Application.Base;
using DDD.Infra.Adapter;
using DDD.Infra.Base;
using DDD.Infra.Session;
using DDD.Infra.UnitOfWork;
using DDD.Infra.Validator;
using DDD.Web.Security;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

namespace DDD.Web
{
    public class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacConfig();
        }

        private static void SetAutofacConfig()
        {
            var builder = new ContainerBuilder();

            var config = GlobalConfiguration.Configuration;

            // Register Controllers
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            //Register Session
            builder.RegisterType<SessionHandler>().As<ISessionHandler>().InstancePerLifetimeScope();

            //Register MainBcUnitOfWork
            builder.RegisterType<MainBcUnitOfWork>().InstancePerLifetimeScope();

            //Register Automapper
            builder.RegisterType<AutomapperTypeAdapterFactory>().As<ITypeAdapterFactory>().InstancePerLifetimeScope();
            
            //Register Repository Factory
            builder.RegisterType<RepositoryFactory>().As<IRepositoryFactory>().InstancePerRequest();

            //Register AppService Factory
            builder.RegisterType<AppServiceFactory>().As<IAppServiceFactory>().InstancePerRequest();
            
            //Build
            builder.RegisterFilterProvider();
            IContainer container = builder.Build();

            var typeAdapterFactory = container.Resolve<ITypeAdapterFactory>();
            TypeAdapterFactory.SetCurrent(typeAdapterFactory);

            //Dependency Resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //Validator
            EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());
        }
    }
}