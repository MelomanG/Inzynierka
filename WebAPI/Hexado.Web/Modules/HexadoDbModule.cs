using Autofac;
using Hexado.Db.Repositories.Specific;

namespace Hexado.Web.Modules
{
    public class HexadoDbModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            InstancePerLifetimeScope(builder);
        }

        private static void InstancePerLifetimeScope(ContainerBuilder builder)
        {
            builder
                .RegisterType<HexadoUserRepository>()
                .As<IHexadoUserRepository>()
                .InstancePerLifetimeScope();
            builder
                .RegisterType<BoardGameRepository>()
                .As<IBoardGameRepository>()
                .InstancePerLifetimeScope();
        }
    }
}