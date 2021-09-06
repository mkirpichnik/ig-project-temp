using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WeddingForward.Api.Extensions;
using WeddingForward.ApplicationServices;
using WeddingForward.ApplicationServices.Accounts.Models;
using WeddingForward.ApplicationServices.Extensions;
using WeddingForward.ApplicationServices.Helpers;
using WeddingForward.Data;
using WeddingForward.Data.Models;

namespace WeddingForward
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<WeddingForwardContext>(options => options.UseSqlServer(connection));

            //Logger.LogInformation(connection);

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            {
                //options.SerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
                options.SerializerSettings.Formatting = Formatting.Indented;
            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddOptions();

            services.Configure<SystemAccountsCollection>(Configuration.GetSection(nameof(SystemAccountsCollection)));

            services.UseApplicationServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Utils.ApplicationLogging.Factory = app.ApplicationServices.GetService<ILoggerFactory>();

            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
