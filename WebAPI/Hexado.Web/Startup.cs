using System.IO;
using Autofac;
using Hexado.Web.Extensions;
using Hexado.Web.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;

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
                .AddControllers()
                .AddNewtonsoftJson(options=> options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
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
            builder.RegisterModule(new HexadoSpeczillaModule());
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(CorsPolicy);
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Images")),
                RequestPath = new PathString("/Images")
            });
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