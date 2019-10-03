using Autofac;
using Hexado.Core.Auth;
using Hexado.Core.Services;

namespace Hexado.Web.Modules
{
    public class HexadoCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HexadoUserService>().As<IHexadoUserService>().SingleInstance();
            builder.RegisterType<JwtTokenFactory>().As<IJwtTokenFactory>().SingleInstance();
        }
    }
}