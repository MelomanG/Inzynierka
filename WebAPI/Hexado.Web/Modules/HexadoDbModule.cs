using Autofac;
using Hexado.Db.Repositories;

namespace Hexado.Web.Modules
{
    public class HexadoDbModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerLifetimeScope();

            builder
                .RegisterType<HexadoUserRepository>()
                .As<IHexadoUserRepository>()
                .InstancePerLifetimeScope();
        }
    }
}