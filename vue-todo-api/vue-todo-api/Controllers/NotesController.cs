﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VueTodoApi.Data;

namespace vue_todo_api.Controllers
{
    [Route("api/[controller]")]
    public class NotesController : Controller
    {
        // GET api/values
        [HttpGet]
        public async Task<string> GetAsync(string userId)
        {
            //return new string[] { "value1", "value2" };
            await Global.notesRepository.CreateNote(1234, "yoo");
            return "oki";
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
