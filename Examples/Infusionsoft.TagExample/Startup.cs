using com.ciclosoftware.infusionsoft.restapi;
using com.ciclosoftware.infusionsoft.restapi.Authorization;
using com.ciclosoftware.infusionsoft.restapi.Contacts;
using com.ciclosoftware.infusionsoft.restapi.Service;
using com.ciclosoftware.infusionsoft.restapi.Tags;
using com.ciclosoftware.infusionsoft.restapi.Users;
using Infusionsoft.TagExample.DomainModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infusionsoft.TagExample
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", p =>
                {
                    p.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            var infClientId = Configuration["InfusionsoftClientId"];
            var infClientSecret = Configuration["InfusionsoftClientSecret"];
            var apiFactory = new ApiFactory(infClientId, infClientSecret);
            services.AddSingleton<IApiFactory>(apiFactory);
            services.AddTransient<IInfusionsoftService, InfusionsoftService>();
            services.AddTransient<IInfusionsoftAuthorization, InfusionsoftAuthorization>();
            services.AddTransient<IInfusionsoftContacts, InfusionsoftContacts>();
            services.AddTransient<IInfusionsoftTags, InfusionsoftTags>();
            services.AddTransient<IInfusionsoftUsers, InfusionsoftUsers>();

            services.AddSingleton<ITagExampleDomainModel, TagExampleDomainModel>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseCors("AllowAll");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
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
