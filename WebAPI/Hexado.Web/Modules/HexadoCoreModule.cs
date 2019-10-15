using System.IdentityModel.Tokens.Jwt;
using Autofac;
using Hexado.Core.Auth;
using Hexado.Core.Services.Specific;
using Hexado.Core.Speczillas;
using Hexado.Web.ActionFilters;
using Microsoft.IdentityModel.Tokens;

namespace Hexado.Web.Modules
{
    public class HexadoCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            InstancePerLifetimeScope(builder);
            SingleInstance(builder);
        }

        private static void SingleInstance(ContainerBuilder builder)
        {
            builder
                .RegisterType<TokenFactory>()
                .As<ITokenFactory>()
                .SingleInstance();
            builder
                .RegisterType<JwtSecurityTokenHandler>()
                .As<ISecurityTokenValidator>()
                .SingleInstance();
            builder
                .RegisterType<AccessTokenValidator>()
                .As<IAccessTokenValidator>()
                .SingleInstance();
            builder
                .RegisterType<BoardGameSpeczilla>()
                .As<IBoardGameSpeczilla>()
                .SingleInstance();
        }

        private static void InstancePerLifetimeScope(ContainerBuilder builder)
        {
            builder
                .RegisterType<AuthService>()
                .As<IAuthService>()
                .InstancePerLifetimeScope();
            builder
                .RegisterType<HexadoUserService>()
                .As<IHexadoUserService>()
                .InstancePerLifetimeScope();
            builder
                .RegisterType<BoardGameService>()
                .As<IBoardGameService>()
                .InstancePerLifetimeScope();
            builder
                .RegisterType<BoardGameCategoryService>()
                .As<IBoardGameCategoryService>()
                .InstancePerLifetimeScope();
            builder
                .RegisterType<RateService>()
                .As<IRateService>()
                .InstancePerLifetimeScope();
            builder
                .RegisterType<AuthorizationHeaderValidation>()
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}