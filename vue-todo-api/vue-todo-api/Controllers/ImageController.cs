using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using VueTodoApi.Models;

namespace VueTodoApi.Controllers
{
    [Route("api/[controller]")]
    public class ImageController : Controller
    {
        readonly IDocumentStore _store;

        public ImageController(IDocumentStore store)
        {
            _store = store;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get(string noteId, string imageName)
        {
            using (var session = _store.OpenSession())
            {
                var res = session.Advanced.Attachments.Get(noteId, imageName);
                if (res == null) return NotFound();
                return File(res.Stream, "image/jpeg"); 
            }
        }
        
        [DisableRequestSizeLimit]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(string noteId, string imageName)
        {
            if (string.IsNullOrWhiteSpace(noteId)) return BadRequest();
            var file = Request.Form.Files[0];
            var size = file.Length;

            if (size > 0)
            {
                using (var session = _store.OpenAsyncSession())
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    var note = await session.LoadAsync<NotesDocument>(noteId);
                    if(note.Images == null) note.Images = new List<string>();
                    if(!note.Images.Contains(imageName)) note.Images.Add(imageName);
                    await session.StoreAsync(note);
                    stream.Seek(0, 0);
                    session.Advanced.Attachments.Store(noteId, imageName, stream, "image/jpeg");
                    await session.SaveChangesAsync();
                    return Ok(new {imageName});
                }
            }
            return BadRequest();
        }
    }
}