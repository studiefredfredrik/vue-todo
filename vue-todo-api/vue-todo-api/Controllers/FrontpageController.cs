using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using System.Linq;
using VueTodoApi.Models;

namespace VueTodoApi.Controllers
{
    [Route("api/[controller]")]
    public class FrontpageController : Controller
    {
        readonly IDocumentStore _store;

        public FrontpageController(IDocumentStore store)
        {
            _store = store;
        }

        [HttpGet]
        public FrontpageDocument Get()
        {
            var defaultFrontPage = GetDefaultFrontpage();
            using (var session = _store.OpenSession())
            {
                var frontpage = session.Query<FrontpageDocument>().FirstOrDefault();
                if (frontpage != null) return frontpage;
            }
            return defaultFrontPage;
        }

        [Authorize]
        [HttpPut]
        public IActionResult Put([FromBody]FrontpageDocument frontpage)
        {
            using (var session = _store.OpenSession())
            {
                var currentFrontpage = session.Query<FrontpageDocument>().FirstOrDefault();
                if (currentFrontpage == null) session.Store(frontpage);
                else
                {
                    session.Delete(currentFrontpage.Id);
                    session.SaveChanges();
                    session.Store(frontpage, currentFrontpage.Id);
                }
                session.SaveChanges();
            }
            return Ok();
        }

        private FrontpageDocument GetDefaultFrontpage()
        {
            return new FrontpageDocument
            {
                Heading = "MY BLOG",
                Undertitle = "Welcome to the blog of ",
                Sidebar = new Sidebar
                {
                    Image = "",
                    Heading = "My Name",
                    Description = "Just me, myself and I, exploring the universe of uknownment. I have a heart of love and a interest of lorem ipsum and mauris neque quam blog. I want to share my world with you."
                }
            };
        }
    }
}
