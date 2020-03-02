using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using AuthenticationTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationTest.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        public IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public IActionResult Login([FromBody]Auth login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if(user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok (new { token = tokenString });
            }

            return response;
        }

        private string GenerateJSONWebToken(Auth userinfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Issuer"], null, expires: DateTime.Now.AddMinutes(120), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Auth AuthenticateUser(Auth login)
        {
            var auth = new List<Auth>()
            {
                new Auth() { Username = "HambaAllah", Password = "rahasia" },
                new Auth() { Username = "userterdaftar", Password = "bandungsupermall" },
                new Auth() { Username = "user3", Password = "secret" },
            };

            Auth user = null;

            if (auth.Any(x => x.Username == login.Username) && auth.Any(x => x.Password == login.Password))
            {
                user = auth.First(x => x.Username == login.Username);
            }

            return user;
        }
    }
}
