using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace VueTodoApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var currentDir = Directory.GetCurrentDirectory();
            var distFolder = Path.Combine(currentDir, "../../vue-todo-frontend/dist");

            return WebHost.CreateDefaultBuilder(args)
            .UseWebRoot(distFolder)
            .UseStartup<Startup>()
            .UseUrls("http://*:9095")
            .Build();

        }
        
    }
}
