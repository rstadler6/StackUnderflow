using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using StackUnderflow.Entities;
using Microsoft.AspNetCore.Identity;
using StackUnderflow.Configuration;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace StackUnderflow.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors]
    public class UserController : ControllerBase
    {
        private readonly JwtConfig _jwtConfig;

        public UserController (IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost]
        [Route("/login")]
        public IActionResult Login([FromBody] User user)
        {
            using (var db = new StackUnderflowContext())
            {
                var existingUser = db.Users.FirstOrDefault(u => u.Username == user.Username);

                if (existingUser == null || existingUser.Password != user.Password)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        ErrorList = new List<string>() 
                        {
                            "Invalid login request"
                        },
                        Success = false
                    });
                }

                var jwtToken = GenerateToken(existingUser.Username);

                return Ok(new RegistrationResponse()
                {
                    Success = true,
                    Token = jwtToken
                });
            }
        }

        
        [HttpPost]
        [Route("/register")]
        public IActionResult Register([FromBody] User user)
        {
            using (var db = new StackUnderflowContext())
            {
                var existingUser = db.Users.FirstOrDefault(u => u.Username == user.Username);

                if (existingUser != null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        ErrorList = new List<string>()
                        {
                            "User already exists"
                        },
                        Success = false
                    });
                }

                db.Users.Add(user);
                db.SaveChangesAsync();
                var jwtToken = GenerateToken(user.Username);

                return Ok(new RegistrationResponse()
                {
                    Success = true,
                    Token = jwtToken
                });
            }
        }

        private string GenerateToken(string username, int expireMinutes = 20)
        {
            var token = JWT.Builder.JwtBuilder.Create()
                      .WithAlgorithm(new JWT.Algorithms.HMACSHA256Algorithm()) // symmetric
                      .WithSecret(_jwtConfig.Secret)
                      .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                      .AddClaim("user", username)
                      .Encode();

            return token;
        }

    }
}
