using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : Controller
    {
        private readonly IConfiguration configuration;
        public TokenController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        [Route("GetToken")]
        [HttpPost]
        public PPW.Models.Token GetToken([FromBody] PPW.Models.User user)
        {
            var token = new PPW.Models.Token();
            var applicationName = user.Username;
            var expirationDateTime = DateTime.Now.AddMinutes(30);
            token.token = CustomTokenJWT(applicationName, expirationDateTime);
            token.expirationTime = expirationDateTime;
            return token;
        }
        private string CustomTokenJWT(string ApplicationName, DateTime token_expiration)
        {
            var _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
            var _signingCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var _Header = new JwtHeader(_signingCredentials);
            var _Claims = new[] {
                //acá es lo que debemos de modificar las los parametros que vamos a utilizar.
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, ApplicationName)
            };
            var _Payload = new JwtPayload(
                    issuer: configuration["JWT:Issuer"],
                    audience: configuration["JWT:Audience"],
                    claims: _Claims,
                    notBefore: DateTime.Now, //cuanto va a durar el token
                    expires: token_expiration    // cuando va a expirar el token.
                );
            var _Token = new JwtSecurityToken(
                    _Header,
                    _Payload
                );
            return new JwtSecurityTokenHandler().WriteToken(_Token);
        }

    }
}
