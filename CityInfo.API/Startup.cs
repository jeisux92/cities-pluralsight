using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace CityInfo.API
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appSettings.json", reloadOnChange: true, optional: false)
                .AddJsonFile($"appSettings.{env.EnvironmentName}.json", reloadOnChange: true, optional: true);

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                    .AddMvcOptions(mvcopt =>
                    {
                        mvcopt.OutputFormatters.Add(
                            new XmlSerializerOutputFormatter()
                        );
                    })
                    .AddJsonOptions(opt =>
            {
                if (opt.SerializerSettings.ContractResolver != null)
                {
                    //var castedRedsolver = (DefaultContractResolver)opt.SerializerSettings.ContractResolver;
                    //castedRedsolver.NamingStrategy = null;
                }
            });

            services.AddTransient<IMailService, LocalMailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            loggerFactory.AddDebug();


            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseStatusCodePages();

            app.UseMvc(configure =>
            {
                //configure.MapRoute(
                //    name: "Default",
                //    template: "{controller}/{action}/{id?}",
                //    defaults: new { controller = "Home", action = "Index" }
                //);
            });

        }
    }
}
