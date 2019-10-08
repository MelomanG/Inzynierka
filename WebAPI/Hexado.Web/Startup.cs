using Autofac;
using Hexado.Web.Extensions;
using Hexado.Web.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hexado.Web
{
    public class Startup
    {
        private const string CorsPolicy = "CORS";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // ====== Read more about this ======
            services
                .AddCors(corsOptions => 
                    corsOptions.AddPolicy(CorsPolicy, policyBuilder =>
                        policyBuilder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()))
                .AddControllers();
            // ====== 

            // ====== Hexado specific ======
            services
                .AddHexadoOptions(Configuration)
                .AddHexadoDbContext()
                .AddHexadoIdentity()
                .AddHexadoAuthentication()
                .AddHexadoAuthorization();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new HexadoCoreModule());
            builder.RegisterModule(new HexadoDbModule());
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(CorsPolicy);
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}