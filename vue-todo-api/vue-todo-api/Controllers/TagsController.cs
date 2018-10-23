using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
                var res = session.Query<TagsDocument>().FirstOrDefault() ?? new TagsDocument {Tags = new List<string>()};
                return Ok(res);
            }
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(string tagsString)
        {
            if (string.IsNullOrWhiteSpace(tagsString)) return Ok();
            var tags = tagsString.Split(",").Where(t => !string.IsNullOrWhiteSpace(t));
            
            using (var session = _store.OpenSession())
            {
                var doc = await session.Query<TagsDocument>().FirstOrDefaultAsync() ?? new TagsDocument {Tags = new List<string>()};

                foreach (var tag in tags)
                {
                    if(doc.Tags.All(t => t != tag)) doc.Tags.Add(tag);    
                }
                
                session.Store(doc);
                session.SaveChanges();
                return Ok();
            }
        }
    }
}