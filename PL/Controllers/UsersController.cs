using DAL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly JWTOptions _jWTOptions;
        public UsersController(JWTOptions jWTOptions)
        {
            _jWTOptions = jWTOptions;
        }
        [HttpPost]
        [Route("Auth")]
        public ActionResult<string> AuthenticateUser(AuthenticationRequest request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jWTOptions.Issuer,
                Audience = _jWTOptions.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTOptions.SigningKey)),
                    SecurityAlgorithms.HmacSha256
                ),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, request.UserName),
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(ClaimTypes.Email, "User@gmail.com")
                })
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return Ok(accessToken);
        }
    }
}
