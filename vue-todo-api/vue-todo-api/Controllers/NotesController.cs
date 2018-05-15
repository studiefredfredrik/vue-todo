using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using VueTodoApi.Configuration;
using VueTodoApi.Models;

namespace VueTodoApi.Controllers
{
    [Route("api/[controller]")]
    public class NotesController : Controller
    {
        readonly IDocumentStore _store;
        readonly NotesSettings _highScoreSettings;

        public NotesController(IDocumentStore store, NotesSettings highScoreSettings)
        {
            _store = store;
            _highScoreSettings = highScoreSettings;
        }

        [HttpGet]
        public List<NotesDocument> Get()
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<NotesDocument>()
                    .Take(100)
                    .OrderByDescending(doc => doc.TimeOfEntry)
                    .ToList();
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]NotesDocument note, string password)
        {
            if (password != _highScoreSettings.SuperSecretApplicationKey) return Forbid();
            if (!string.IsNullOrEmpty(note.Id)) return BadRequest("Note Id is already populated");

            note.TimeOfEntry = DateTime.Now;
            using (var session = _store.OpenSession())
            {
                session.Store(note);
                session.SaveChanges();
            }
            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody]NotesDocument note, string password)
        {
            if (password != _highScoreSettings.SuperSecretApplicationKey) return Forbid();
            if (string.IsNullOrEmpty(note.Id)) return BadRequest("Note does not have an Id");

            note.TimeOfEntry = DateTime.Now;
            using (var session = _store.OpenSession())
            {
                session.Store(note);
                session.SaveChanges();
            }
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(string id, string password)
        {
            if (password != _highScoreSettings.SuperSecretApplicationKey) return Forbid();

            using (var session = _store.OpenSession())
            {
                session.Delete(id);
                session.SaveChanges();
            }
            return Ok();
        }
    }
}
