using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackUnderflow.Entities;
using StackUnderflow.Configuration;
using Microsoft.Extensions.Options;

namespace StackUnderflow.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly JwtConfig _jwtConfig;
        private readonly List<JwtValues> _jwtList;

        public UserController(IOptionsMonitor<JwtConfig> optionsMonitor, ILogger<UserController> logger, List<JwtValues> jwtList)
        {
            _jwtConfig = optionsMonitor.CurrentValue; 
            _logger = logger;
            _jwtList = jwtList;
        }

        [HttpPost]
        [Route("/login")]
        public IActionResult Login([FromBody] User user)
        {
            _logger.LogInformation($"Login called, User: {user.Username}");

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
            _logger.LogInformation($"Register called, User: {user.Username}");

            using (var db = new StackUnderflowContext())
            {
                var existingUser = db.Users.FirstOrDefault(u => u.Username == user.Username);

                if (existingUser != null)
                {
                    _logger.LogInformation($"Register failed, User {user.Username} already exists");

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

        private string GenerateToken(string username, int expireHours = 1)
        {
            var token = JWT.Builder.JwtBuilder.Create()
                      .WithAlgorithm(new JWT.Algorithms.HMACSHA256Algorithm()) // symmetric
                      .WithSecret(_jwtConfig.Secret)
                      .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(expireHours).ToUnixTimeSeconds())
                      .AddClaim("user", username)
                      .
                      .Encode();
            return token;
        }
    }
}