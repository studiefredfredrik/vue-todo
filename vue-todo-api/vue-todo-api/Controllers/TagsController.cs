using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
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
                var res = session.Query<TagsDocument>().FirstOrDefaultAsync();
                return Ok(res);
            }
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag)) return BadRequest();
            using (var session = _store.OpenSession())
            {
                var doc = await session.Query<TagsDocument>().FirstOrDefaultAsync();
                if (doc == null) doc = new TagsDocument {Tags = new List<string>()};
                if (doc.Tags.Any(t => t == tag)) return Ok();
                doc.Tags.Add(tag);
                session.Store(doc);
                session.SaveChanges();
                return Ok();
            }
        }

        [Authorize]
        [HttpPut]
        public IActionResult Put([FromBody]NotesDocument note)
        {
            if (string.IsNullOrEmpty(note.Id)) return BadRequest("Note does not have an Id");

            using (var session = _store.OpenSession())
            {
                session.Store(note);
                session.SaveChanges();
                return Ok(new { id = note.Id});
            }
        }

        [Authorize]
        [HttpDelete]
        public IActionResult Delete(string id)
        {
            using (var session = _store.OpenSession())
            {
                session.Delete(id);
                session.SaveChanges();
            }
            return Ok();
        }
    }
    }
}