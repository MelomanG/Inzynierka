using System;
using System.Text;
using Hexado.Db;
using Hexado.Db.Entities;
using Hexado.Web.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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
                .AddEntityFrameworkStores<HexadoDbContext>();

            return services;
        }

        public static IServiceCollection AddHexadoAuthentication(this IServiceCollection services)
        {
            var jwtOptions = services.BuildServiceProvider()
                .GetRequiredService<IOptions<JwtOptions>>().Value;

            var key = Encoding.UTF8.GetBytes(jwtOptions.Secret);

            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = false;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }
    }
}