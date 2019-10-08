using Hexado.Core.Auth;
using Hexado.Db;
using Hexado.Db.Entities;
using Hexado.Web.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Hexado.Web.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddHexadoOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SqlServerDbOptions>(configuration.GetSection(SqlServerDbOptions.SectionName));
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

            return services;
        }

        public static IServiceCollection AddHexadoDbContext(this IServiceCollection services)
        {
            var sqlServerDbOptions = services.BuildServiceProvider()
                .GetRequiredService<IOptions<SqlServerDbOptions>>().Value;

            services.AddDbContext<HexadoDbContext>(opt =>
                opt.UseSqlServer(sqlServerDbOptions.ConnectionString));

            return services;
        }

        public static IServiceCollection AddHexadoIdentity(this IServiceCollection services)
        {
            services
                .Configure<IdentityOptions>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 4;
                    options.User.RequireUniqueEmail = true;
                })
                .AddDefaultIdentity<HexadoUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<HexadoDbContext>();

            return services;
        }

        public static IServiceCollection AddHexadoAuthentication(this IServiceCollection services)
        {
            var jwtOptions = services.BuildServiceProvider()
                .GetRequiredService<IOptions<JwtOptions>>().Value;

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = HexadoTokenSpecific.GetValidationParameters(jwtOptions.Secret);
                });

            return services;
        }
    }
}