using System.IdentityModel.Tokens.Jwt;

namespace VueTodoApi.Controllers
{
    internal class JwtToken
    {
        private JwtSecurityToken token;

        public JwtToken(JwtSecurityToken token)
        {
            this.token = token;
        }
    }
}