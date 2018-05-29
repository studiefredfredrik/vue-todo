using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VueTodoApi.Middleware
{
    public class TokenParserMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenParserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            // Define const Key this should be private secret key  stored in some safe place
            string key = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";

            // Create Security key  using private key above:
            // not that latest version of JWT using Microsoft namespace instead of System
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            // Also note that securityKey length should be >256b
            // so you have to make sure that your private key has a proper length
            //
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //  Finally create a Token
            var header = new JwtHeader(credentials);

            var handler = new JwtSecurityTokenHandler();


            // And finally when  you received token from client
            // you can  either validate it or try to  read
            string jwtToken = httpContext.Request.Cookies["token"];

            if(string.IsNullOrEmpty(jwtToken)) return _next(httpContext);

            var token = handler.ReadJwtToken(jwtToken);

            httpContext.Items["username"] = token.Payload.First().Value;

            return _next(httpContext);
        }
    }
}
