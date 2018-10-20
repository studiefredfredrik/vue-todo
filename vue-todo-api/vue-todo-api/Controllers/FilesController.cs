using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Raven.Client.Documents;
using VueTodoApi.Configuration;

namespace VueTodoApi.Controllers
{
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        private static string filesPath;
        
        public FilesController(FilesSettings filesSettings, IDocumentStore store, IHostingEnvironment hostingEnvironment)
        {
            if (!string.IsNullOrWhiteSpace(filesSettings.Path)) filesPath = filesSettings.Path;
            else filesPath = hostingEnvironment.WebRootPath;
        }

        [DisableRequestSizeLimit]
        [Authorize]
        [HttpGet("{folder}/{fileName}")]
        public IActionResult GetFile(string folder, string fileName)
        {
            var path = $"{filesPath}/{folder}/{fileName}";

            if (path.Contains("..")) return Forbid();
            if (!System.IO.File.Exists(path)) return NotFound();

            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out var contentType);
            return File(System.IO.File.Open(path, FileMode.Open), contentType);

        }
        
        [DisableRequestSizeLimit]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(string folder, string fileName, bool overwrite = false)
        {
            if (string.IsNullOrWhiteSpace(folder) || string.IsNullOrWhiteSpace(fileName)) return BadRequest();
            
            var path = $"{filesPath}/{folder}/{fileName}";

            if (path.Contains("..")) return Forbid();

            if (System.IO.File.Exists(path))
            {
                if(overwrite) System.IO.File.Delete(path);
                else return Forbid();
            }

            if (!Directory.Exists($"{filesPath}/{folder}"))
                Directory.CreateDirectory($"{filesPath}/{folder}");
            
            var file = Request.Form.Files[0];
            var size = file.Length;

            if (size > 0)
            {
                using (var fileStream = System.IO.File.Create(path))
                {
                    fileStream.Seek(0, SeekOrigin.Begin);
                    await file.CopyToAsync(fileStream);
                    fileStream.Close();
                }
                return Ok(new {fileName});
            }
            return BadRequest();
        }
    }
}