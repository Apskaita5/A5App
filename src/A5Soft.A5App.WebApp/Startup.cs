using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using A5Soft.A5App.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using A5Soft.A5App.WebApp.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using BlazorStrap;
using Microsoft.AspNetCore.Localization;

namespace A5Soft.A5App.WebApp
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;


        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }


        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

            services.AddTransient<ClaimsIdentity>(s =>
                (ClaimsIdentity)s.GetService<AuthenticationStateProvider>()?
                    .GetAuthenticationStateAsync().Result.User?.Identity);

            services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, AppAuthenticationHandler>(
                AppAuthenticationHandler.AuthSchemaName, _ => { });

            services.AddBootstrapCss();

            services.AddApplicationServices(Configuration, _env);

            services.AddControllers();
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.Remove(new ServiceDescriptor(typeof(AuthenticationStateProvider),
                typeof(ServerAuthenticationStateProvider), ServiceLifetime.Singleton));
            services.Remove(new ServiceDescriptor(typeof(AuthenticationStateProvider),
                typeof(ServerAuthenticationStateProvider), ServiceLifetime.Scoped));
            services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();

            services.AddSingleton<WeatherForecastService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var supportedCultures = SupportedLocales.All
                .Select(l => l.Culture).ToList();
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(SupportedLocales.Default.Name),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                // Add a new endpoint that uses the VersionMiddleware
                endpoints.Map("/DataPortal", endpoints.CreateApplicationBuilder()
                        .UseMiddleware<DataPortalMiddleware>()
                        .Build())
                    .WithDisplayName("Version number");
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
