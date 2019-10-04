using System.IdentityModel.Tokens.Jwt;
using Autofac;
using Hexado.Core.Auth;
using Hexado.Core.Services;
using Hexado.Web.ActionFilters;

namespace Hexado.Web.Modules
{
    public class HexadoCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<HexadoUserService>()
                .As<IHexadoUserService>()
                .SingleInstance();
            builder
                .RegisterType<AuthService>()
                .As<IAuthService>()
                .SingleInstance();
            builder
                .RegisterType<TokenFactory>()
                .As<ITokenFactory>()
                .SingleInstance();
            builder
                .RegisterType<JwtSecurityTokenHandler>()
                .AsSelf()
                .InstancePerLifetimeScope();
            builder
                .RegisterType<AccessTokenValidator>()
                .As<IAccessTokenValidator>()
                .SingleInstance();
            builder
                .RegisterType<AuthorizationHeaderValidation>()
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}