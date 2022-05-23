using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnventisTT.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        public IConfiguration _configuration;

        public LoginController(IConfiguration config)
        {
            _configuration = config;
        }

        [HttpPost]
        public IActionResult Post(string login, string password)
        {
            if (login == _configuration["Jwt:Login"] && password == _configuration["Jwt:Password"])
            {
                var claims = new[] {new Claim("Login", login)};

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signIn);

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            else
            {
                return BadRequest("Invalid credentials");
            }
        }
    }
}
