using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Raven.Client.Documents;
using System.IO;

namespace vue_todo_api
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
            services.AddMvc();
            var store = InitRavenDb();
            services.AddSingleton(store);
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

            app.UseMvc();
        }

        private static IDocumentStore InitRavenDb()
        {
            var store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "vue-todo-notes"
            };
            store.Initialize();
            return store;
        }

    }
}
