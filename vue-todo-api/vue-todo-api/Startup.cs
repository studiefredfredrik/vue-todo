using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IO;
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

            var notesSettings = Configuration.GetSection("NotesSettings").Get<NotesSettings>();
            services.AddSingleton(notesSettings);

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

            var option = new RewriteOptions();
            option.AddRedirect("^$", "index.html"); // Matches on empty string
            app.UseRewriter(option);

            var currentDir = Directory.GetCurrentDirectory();
            var distFolder = Path.Combine(currentDir, "../../vue-todo-frontend/dist");
            var staticFileOptions = new StaticFileOptions();
            staticFileOptions.FileProvider = new PhysicalFileProvider(distFolder);
            app.UseStaticFiles(staticFileOptions);

            app.UseDeveloperExceptionPage();
            app.UseAuthentication();


            app.UseMvc();
        }
    }
}
