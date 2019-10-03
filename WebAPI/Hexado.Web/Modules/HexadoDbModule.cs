using Autofac;
using Hexado.Db;

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
        }
    }
}