using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Raven.Client.Documents;
using VueTodoApi.Data;

namespace vue_todo_api
{
    public static class Global
    {
        public static NotesRepository notesRepository;
    }
    

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
            }

            //var option = new RewriteOptions();
            //option.AddRedirect("^$", "index.html");
            //app.UseRewriter(option);

            app.UseStaticFiles();
            app.UseMvc();

            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "../../vue-todo-frontend/dist")), RequestPath = ""
            //});

            var documentStore = new DocumentStore()
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "vue-todo-notes"
            }.Initialize();

        }
    }
}
