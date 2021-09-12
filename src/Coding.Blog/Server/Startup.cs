using System;
using Autofac;
using Coding.Blog.Server.CompositionRoot;
using Coding.Blog.Server.Configurations;
using Coding.Blog.Server.HostedServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Coding.Blog.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            services.AddControllersWithViews();
            services.AddRazorPages();

            var applicationLifeTimeConfiguration = new ApplicationLifetimeConfiguration();

            Configuration.GetSection(ApplicationLifetimeConfiguration.Key).Bind(applicationLifeTimeConfiguration);
            services.AddSingleton(applicationLifeTimeConfiguration);
            services.AddHostedService<ApplicationLifetimeService>();

            services.Configure<HostOptions>(opts =>
                opts.ShutdownTimeout = TimeSpan.FromSeconds(
                    applicationLifeTimeConfiguration.ApplicationShutdownTimeoutSeconds
                )
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHealthChecks("/healthz");
            app.UseForwardedHeaders();
            app.Use(async (context, next) =>
            {
                if (context.Request.IsHttps || context.Request.Headers["X-Forwarded-Proto"] == Uri.UriSchemeHttps)
                {
                    await next();
                }
                else
                {
                    var queryString = context.Request.QueryString.HasValue
                        ? context.Request.QueryString.Value
                        : string.Empty;
                    var https = "https://" + context.Request.Host + context.Request.Path + queryString;

                    context.Response.Redirect(https, true);
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }

        public void ConfigureContainer(ContainerBuilder builder) => ContainerConfigurator.Configure(builder);
    }
}
