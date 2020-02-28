using System;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using AuthenticationTest.Models;

namespace RayhanASPRestTest.Controllers
{

    [ApiController]
    [Route("user")]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult Authenticate(Auth user)
        {
            var auth = new List<Auth>()
            {
                new Auth() { Username = "HambaAllah", Password = "rahasia" },
                new Auth() { Username = "userterdaftar", Password = "bandungsupermall" },
                new Auth() { Username = "user3", Password = "secret" },
            };

            var _user = auth.Find(x => x.Username == user.Username);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDesc = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, _user.Username),
                    new Claim(ClaimTypes.Sid, _user.Password)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("secrethastobelongerthanitshouldbe")), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDesc);

            var tokenResponse = new
            {
                token = tokenHandler.WriteToken(token),
                user = _user.Username
            };

            return Ok(tokenResponse);
        }
    }
}
