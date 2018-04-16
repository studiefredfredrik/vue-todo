using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using VueTodoApi;


namespace vue_todo_api.Controllers
{
    [Route("api/[controller]")]
    public class NotesController : Controller
    {
        IDocumentStore _dc;
        public NotesController(IDocumentStore dc)
        {
            _dc = dc;
        }

        [HttpGet]
        public async Task<List<NotesDocument>> GetAsync()
        {
            int userId = 100;
            using (var session = _dc.OpenAsyncSession())
            {
                var result = await session.Query<NotesDocument>()
                    .Where(document => document.UserId == userId)
                    .ToListAsync();

                return result;
            }
        }

        [HttpPost]
        public async Task PostAsync([FromBody]string text)
        {
            int userId = 100;
            using (var session = _dc.OpenAsyncSession())
            {
                var doc = new NotesDocument
                {
                    Text = text,
                    UserId = userId,
                    NoteId = Guid.NewGuid()
                };
                await session.StoreAsync(doc);
                await session.SaveChangesAsync();
            }
        }

        public class NotesDocument
        {
            public int UserId { get; set; }
            public string Text { get; set; }
            public Guid NoteId { get; set; }
        }
    }
}
