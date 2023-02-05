using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        [HttpPost, Route("login")]
        public IActionResult Login(LoginDTO loginDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(loginDTO.UserName) || string.IsNullOrEmpty(loginDTO.Password))
                {
                    return BadRequest("Username and/or Password not specified");
                }
                    
                if (loginDTO.UserName.Equals("joydip") && loginDTO.Password.Equals("joydip123"))
                {
                    var claims = new List<Claim> 
                    { 
                        new Claim(ClaimTypes.Name, loginDTO.UserName),
                        new Claim(ClaimTypes.Name, loginDTO.Password)
                    };

                    var jwtSecurityToken = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        claims: claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(10)),
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                    return Ok(tokenString);
                }
            }
            catch
            {
                return BadRequest
                ("An error occurred in generating the token");
            }

            return Unauthorized();
        }
    }
}
