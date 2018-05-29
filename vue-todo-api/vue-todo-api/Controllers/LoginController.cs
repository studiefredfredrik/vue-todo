using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using static VueTodoApi.Controllers.LoginController.JwtTokenBuilder;

namespace VueTodoApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        [HttpPost]
        public IActionResult Post([FromBody] LoginRequest login)
        {
            if(login.Username == "user" && login.Password == "password")
            {
                //SetCookie(login.Username);
                var token = new JwtTokenBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create("fiversecret "))
                    .AddSubject("james bond")
                    .AddIssuer("Fiver.Security.Bearer")
                    .AddAudience("Fiver.Security.Bearer")
                    .AddClaim("MembershipId", "111")
                    .AddExpiry(1)
                    .Build();

                return Ok(token.Value);
            }
            return Forbid();
        }

        public class LoginRequest
        {
            public string Username;
            public string Password;
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

        public sealed class JwtTokenBuilder
        {
            private SecurityKey securityKey = null;
            private string subject = "";
            private string issuer = "";
            private string audience = "";
            private Dictionary<string, string> claims = new Dictionary<string, string>();
            private int expiryInMinutes = 5;

            public JwtTokenBuilder AddSecurityKey(SecurityKey securityKey)
            {
                this.securityKey = securityKey;
                return this;
            }

            public JwtTokenBuilder AddSubject(string subject)
            {
                this.subject = subject;
                return this;
            }

            public JwtTokenBuilder AddIssuer(string issuer)
            {
                this.issuer = issuer;
                return this;
            }

            public JwtTokenBuilder AddAudience(string audience)
            {
                this.audience = audience;
                return this;
            }

            public JwtTokenBuilder AddClaim(string type, string value)
            {
                this.claims.Add(type, value);
                return this;
            }

            public JwtTokenBuilder AddClaims(Dictionary<string, string> claims)
            {
                this.claims.Union(claims);
                return this;
            }

            public JwtTokenBuilder AddExpiry(int expiryInMinutes)
            {
                this.expiryInMinutes = expiryInMinutes;
                return this;
            }

            public JwtToken Build()
            {
                EnsureArguments();

                var claims = new List<Claim>
            {
              new Claim(JwtRegisteredClaimNames.Sub, this.subject),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }
                .Union(this.claims.Select(item => new Claim(item.Key, item.Value)));

                var token = new JwtSecurityToken(
                                  issuer: this.issuer,
                                  audience: this.audience,
                                  claims: claims,
                                  expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                                  signingCredentials: new SigningCredentials(
                                                            this.securityKey,
                                                            SecurityAlgorithms.HmacSha256));

                return new JwtToken(token);
            }

            private void EnsureArguments()
            {
                if (this.securityKey == null)
                    throw new ArgumentNullException("Security Key");

                if (string.IsNullOrEmpty(this.subject))
                    throw new ArgumentNullException("Subject");

                if (string.IsNullOrEmpty(this.issuer))
                    throw new ArgumentNullException("Issuer");

                if (string.IsNullOrEmpty(this.audience))
                    throw new ArgumentNullException("Audience");
            }

            public sealed class JwtToken
            {
                private JwtSecurityToken token;

                internal JwtToken(JwtSecurityToken token)
                {
                    this.token = token;
                }

                public DateTime ValidTo => token.ValidTo;
                public string Value => new JwtSecurityTokenHandler().WriteToken(this.token);
            }

            public static class JwtSecurityKey
            {
                public static SymmetricSecurityKey Create(string secret)
                {
                    return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
                }
            }

        }
    }


}
