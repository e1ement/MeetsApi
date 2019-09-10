using Meets.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Meets.API
{
    public partial class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureAutoMapper();
            services.ConfigureIisIntegration();
            services.ConfigureLoggerService();
            services.ConfigureDatabaseConnection(Configuration);
            services.ConfigureRepositoryWrapper();
            services.ConfigureSwagger();
            services.ConfigureAuthentication(Configuration);
            services.ConfigureSeedData();
            services.ConfigureLogActivity();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = 
                            Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });
        }
    }
}
