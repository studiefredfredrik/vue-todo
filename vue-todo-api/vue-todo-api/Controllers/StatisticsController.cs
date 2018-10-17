using System.Linq;
using System.Threading.Tasks;
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


        [HttpGet]
        public IActionResult GetTop4()
        {
            using (var session = _store.OpenSession())
            {
                var doc = session.Query<StatisticsDocument>()
                                 .OrderBy(stats => stats.Views)
                                 .Take(4)
                                 .ToList();
                              
                return Ok(doc);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> NoteViewed(string noteId)
        {
            using (var session = _store.OpenSession())
            {
                var doc = await session.Query<StatisticsDocument>()
                              .FirstOrDefaultAsync(note => note.Id == noteId) 
                                ?? new StatisticsDocument {Id = noteId, Views = 0};
                doc.Views++;
                session.Store(doc);
                session.SaveChanges();
                return Ok();
            }
        }

    }
    
}