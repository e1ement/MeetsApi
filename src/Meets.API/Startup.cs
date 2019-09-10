using System.IO;
using Contracts;
using Meets.API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using NLog;

namespace Meets.API
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerManager logger, ISeed seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.Use(async (context, next) =>
                {
                    await next();
                    if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                    {
                        context.Request.Path = "/index.html";
                        await next();
                    }
                });
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FoxTrading API V1");
                c.RoutePrefix = string.Empty;
            });
            app.ConfigureCustomExceptionMiddleware();
            seeder.SeedValues(); //TODO: Comment before publishing
            seeder.SeedUsers(); //TODO: Comment before publishing
            app.UseCors("CorsPolicy");
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapSpaFallbackRoute(
                    "spa-fallback",
                    new { controller = "Fallback", action = "Index"});
            });
        }
    }
}
