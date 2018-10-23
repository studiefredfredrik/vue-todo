using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using VueTodoApi.Models;

namespace VueTodoApi.Controllers
{
    [Route("api/[controller]")]
    public class NotesController : Controller
    {
        readonly IDocumentStore _store;

        public NotesController(IDocumentStore store)
        {
            _store = store;
        }

        [HttpGet]
        public IActionResult Get(int pageSize, int pageNumber, string tag = null)
        {
            if (pageSize > 100) return Forbid();
            var tagFilter = !string.IsNullOrWhiteSpace(tag);
            using (var session = _store.OpenSession())
            {
                var res = new List<NotesDocument>();
                if (tagFilter)
                {
                    res = session.Query<NotesDocument>()
                        .Where(note => note.Tags != null && note.Tags.Any(t => t == tag))
                        .OrderByDescending(doc => doc.TimeOfEntry)
                        .Skip(pageNumber * pageSize)
                        .Take(pageSize)
                        .ToList();
                }
                else
                {
                    res = session.Query<NotesDocument>()
                        .OrderByDescending(doc => doc.TimeOfEntry)
                        .Skip(pageNumber * pageSize)
                        .Take(pageSize)
                        .ToList();    
                }
                
                return Ok(res);
            }
        }
        
        
        [HttpGet("count")]
        public int GetCount()
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<NotesDocument>().Count();
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody]NotesDocument note)
        {
            if (!string.IsNullOrEmpty(note.Id)) return BadRequest("Note Id is already populated");

            note.TimeOfEntry = DateTime.Now;
            using (var session = _store.OpenSession())
            {
                session.Store(note);
                session.SaveChanges();
                return Ok(new { id = note.Id});
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
