using Autofac;
using Hexado.Db.Repositories;
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
                .RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerLifetimeScope();
            builder
                .RegisterType<HexadoUserRepository>()
                .As<IHexadoUserRepository>()
                .InstancePerLifetimeScope();
            builder
                .RegisterType<BoardGameRepository>()
                .As<IBoardGameRepository>()
                .InstancePerLifetimeScope();
            builder
                .RegisterType<PubRepository>()
                .As<IPubRepository>()
                .InstancePerLifetimeScope();
            builder
                .RegisterType<ContactRepository>()
                .As<IContactRepository>()
                .InstancePerLifetimeScope();
            builder
                .RegisterType<EventRepository>()
                .As<IEventRepository>()
                .InstancePerLifetimeScope();
        }
    }
}