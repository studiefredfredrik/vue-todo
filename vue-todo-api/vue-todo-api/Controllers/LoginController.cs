using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace VueTodoApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        public IActionResult Login(string requestPath)
        {
            ViewBag.RequestPath = requestPath ?? "/";
            return View();
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> PostAsync([FromForm] LoginInputModel login)
        {
            if(login.Username == "user" && login.Password == "password")
            {
                //SetCookie(login.Username);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, login.Username)
                };

                var userIdentity = new ClaimsIdentity(claims, "login");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);

                return Redirect("/");
            }
            return Forbid();
        }

        public class LoginInputModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        //private void SetCookie(string username, int expireInMinutes = 60 * 24)
        //{
        //    var token = GenerateToken(username);
        //    CookieOptions option = new CookieOptions();
        //    option.Expires = DateTime.Now.AddMinutes(expireInMinutes);
        //    Response.Cookies.Append("token", token, option);
        //    HttpContext.Response.Cookies.Append("token", token, option);
        //}

        //private string GenerateToken(string username)
        //{
        //    // Define const Key this should be private secret key  stored in some safe place
        //    string key = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";

        //   // Create Security key  using private key above:
        //   // not that latest version of JWT using Microsoft namespace instead of System
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        //    // Also note that securityKey length should be >256b
        //    // so you have to make sure that your private key has a proper length
        //    //
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        //    //  Finally create a Token
        //    var header = new JwtHeader(credentials);

        //    //Some PayLoad that contain information about the  customer
        //    var payload = new JwtPayload
        //    {
        //        { "username ", username}
        //    };

        //    //
        //    var secToken = new JwtSecurityToken(header, payload);
        //    var handler = new JwtSecurityTokenHandler();

        //    // Token to String so you can use it in your client
        //    var tokenString = handler.WriteToken(secToken);

        //    return tokenString;
        //}



    }
}
