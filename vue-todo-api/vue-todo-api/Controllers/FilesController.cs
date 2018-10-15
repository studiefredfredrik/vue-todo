using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Raven.Client.Documents;
using VueTodoApi.Configuration;
using VueTodoApi.Models;

namespace VueTodoApi.Controllers
{
    public class FilesController : Controller
    {
        private readonly FilesSettings _filesSettings;
        readonly IDocumentStore _store;
        
        public FilesController(FilesSettings filesSettings, IDocumentStore store)
        {
            _filesSettings = filesSettings;
            _store = store;
        }

        [DisableRequestSizeLimit]
        [Authorize]
        [HttpGet("{noteId}/{fileName}")]
        public IActionResult GetFile(string noteId, string fileName)
        {
            var path = $"{_filesSettings.Path}/{noteId}/{fileName}";

            if (path.Contains("..")) return Forbid();
            if (!System.IO.File.Exists(path)) return NotFound();

            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out var contentType);
            return File(System.IO.File.Open(path, FileMode.Open), contentType);

        }
        
        [DisableRequestSizeLimit]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(string noteId, string fileName)
        {
            if (string.IsNullOrWhiteSpace(noteId) || string.IsNullOrWhiteSpace(fileName)) return BadRequest();
            
            var path = $"{_filesSettings.Path}/{noteId}/{fileName}";

            if (path.Contains("..")) return Forbid();
            if (System.IO.File.Exists(path)) return Forbid();
            
            var file = Request.Form.Files[0];
            var size = file.Length;

            if (size > 0)
            {
                using (var fileStream = System.IO.File.Create(path))
                using (var session = _store.OpenAsyncSession())
                {
                    
                    var note = await session.LoadAsync<NotesDocument>(noteId);
                    if(note.Files == null) note.Files = new List<string>();
                    if(!note.Files.Contains(fileName)) note.Files.Add(fileName);
                    await session.StoreAsync(note);
                    await session.SaveChangesAsync();

                    fileStream.Seek(0, SeekOrigin.Begin);
                    await file.CopyToAsync(fileStream);
                    fileStream.Close();
                    
                    return Ok(new {fileName});
                }
            }
            return BadRequest();
        }
    }
}