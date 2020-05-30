using Autofac;

namespace Hexado.Web.Modules
{
    public class HexadoSpeczillaModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            InstancePerLifetimeScope(builder);
        }

        private static void InstancePerLifetimeScope(ContainerBuilder builder)
        {
            ////////////////////////////////////////////
        }
    }
}