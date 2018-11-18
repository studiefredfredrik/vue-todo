using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using VueTodoApi.Models;

namespace VueTodoApi.Controllers
{
    [Route("api/[controller]")]
    public class TagsController : Controller
    {
        readonly IDocumentStore _store;

        public TagsController(IDocumentStore store)
        {
            _store = store;
        }

        [HttpGet]
        public IActionResult Get()
        {
            using (var session = _store.OpenSession())
            {
                var list = session.Query<NotesDocument>()
                    .GroupByArrayValues(t => t.Tags)
                    .Select(grp => new
                        {
                            Tag = grp.Key,
                            Count = grp.Count()
                        })
                    .OrderByDescending(x => x.Count)
                    .ToList();

                list.RemoveAll(t => string.IsNullOrWhiteSpace(t.Tag));
                return Ok(list);
            }
        }
        
    }
}