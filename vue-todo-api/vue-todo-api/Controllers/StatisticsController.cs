using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VueTodoApi.Models;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;

namespace VueTodoApi.Controllers
{

    [Route("api/[controller]")]
    public class StatisticsController : Controller
    {
        readonly IDocumentStore _store;

        public StatisticsController(IDocumentStore store)
        {
            _store = store;
        }


        [HttpGet("top4")]
        public IActionResult GetTop4()
        {
            using (var session = _store.OpenSession())
            {
                var doc = session.Query<NotesDocument>()
                     .OrderByDescending(stats => stats.Views)
                     .Take(4)
                     .ToList();
                              
                return Ok(doc);
            }
        }
        
        [HttpPost]
        public IActionResult NoteViewed(string noteId)
        {
            if (string.IsNullOrWhiteSpace(noteId)) return Forbid();
            using (var session = _store.OpenSession())
            {
                var doc = session.Load<NotesDocument>(noteId);
                if (doc == null) return NotFound();
                doc.Views++;
                session.Store(doc);
                session.SaveChanges();
                return Ok();
            }
        }

    }
    
}