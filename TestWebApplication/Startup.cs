using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin.Builder;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using Owin.Security.AesDataProtectorProvider;
using Owin.Security.AesDataProtectorProvider.CrypticProviders;

namespace TestWebApplication
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
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseOwinAppBuilder(builder =>
            {
                builder.SetDataProtectionProvider(new AesDataProtectorProvider(new Sha512ManagedFactory(), new Sha256ManagedFactory(), new AesManagedFactory(), "randomkey"));
                builder.RunSignalR(new HubConfiguration());
            });

            var hbt = new HeartbeatTest();
            Console.WriteLine(hbt.GetType() + " started");
        }
    }

    public static class AppBuilderHelpers
    {
        public static IApplicationBuilder UseOwinAppBuilder(this IApplicationBuilder app, Action<IAppBuilder> owinConfiguration)
        {
            app.UseOwin(
                addToPipeline =>
                {
                    addToPipeline(
                        next =>
                        {
                            var builder = new AppBuilder();

                            owinConfiguration(builder);

                            builder.Run(ctx => next(ctx.Environment));

                            var appFunc = (Func<IDictionary<string, object>, Task>)builder.Build(typeof(Func<IDictionary<string, object>, Task>));

                            return appFunc;
                        });
                });
            return app;
        }
    }
}
