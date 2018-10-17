using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using VueTodoApi.Configuration;

namespace VueTodoApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var serilogSettings = Configuration.GetSection("SerilogSettings").Get<SerilogSettings>();
            SerilogConfiguration.Configure(serilogSettings);

            var ravenConfig = Configuration.GetSection("RavenDbSettings").Get<RavenDbSettings>();
            services.AddSingleton(RavenDbConfiguration.Configure(ravenConfig));
            
            var filesConfig = Configuration.GetSection("FileSettings").Get<FilesSettings>();
//            if(string.IsNullOrWhiteSpace(filesConfig.Path)) filesConfig.Path = hostingEnvironment.WebRootPath;
            services.AddSingleton(filesConfig);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => {
                    options.Events.OnRedirectToLogin = (context) =>
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    };
                });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseDeveloperExceptionPage();
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
