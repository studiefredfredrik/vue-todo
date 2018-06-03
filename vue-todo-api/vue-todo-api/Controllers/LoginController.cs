using BCrypt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VueTodoApi.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        readonly IDocumentStore _store;

        public LoginController(IDocumentStore store)
        {
            _store = store;
        }

        [Route("api/login")]
        public async Task<IActionResult> Login()
        {
            if (await UserExists("administrator") == false)
            {
                return View("SetPassword");
            }
            return View("Login");
        }

        [Route("api/login")]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromForm] LoginInputModel login)
        {
            // Only support one user at the moment
            if(login.Username != "administrator") return Redirect("/api/login");

            // Setting the admin password if not exists
            if (await UserExists(login.Username) != true)
            {
                await CreateUser(login.Username, login.Password);
            }

            if(await VerifyUserLogin(login.Username, login.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, login.Username)
                };

                var userIdentity = new ClaimsIdentity(claims, "login");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);

                // Also set a non-HttpOnly cookie for reading in javascript (not secure)
                Response.Cookies.Append("user", login.Username);

                return Redirect("/");
            }
            return Forbid();
        }

        [Route("api/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            Response.Cookies.Delete("login");

            return Redirect("/");
        }

        private async Task<bool> CreateUser(string username, string password)
        {
            if (await UserExists(username)) return false; // User already exists
            using (var session = _store.OpenAsyncSession())
            {
                var user = new AppUser {
                    Username = username,
                    PasswordHash = BCryptHelper.HashPassword(password, BCryptHelper.GenerateSalt())
                };
                await session.StoreAsync(user);
                await session.SaveChangesAsync();
            }
            return true;
        }

        private async Task<bool> UserExists(string username)
        {
            using (var session = _store.OpenAsyncSession())
            {
                var user = await session.Query<AppUser>()
                    .FirstOrDefaultAsync(u => u.Username == username);
                return user != null;
            }
        }

        private async Task<bool> VerifyUserLogin(string username, string password)
        {
            using (var session = _store.OpenAsyncSession())
            {
                var user = await session.Query<AppUser>()
                    .FirstOrDefaultAsync(u => u.Username == username);
                return BCryptHelper.CheckPassword(password, user.PasswordHash);
            }
        }

        private async Task<AppUser> GetUserAsync(string username)
        {
            using (var session = _store.OpenAsyncSession())
            {
                return await session.Query<AppUser>().FirstOrDefaultAsync();
            }
        }

        public class AppUser
        {
            public string Username { get; set; }
            public string PasswordHash { get; set; }
        }


        public class LoginInputModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

    }
}
