using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackUnderflow.Entities;

namespace StackUnderflow.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [Route("/login")]
        public string Login(User user)
        {
            return string.Empty;
        }

        [HttpPost]
        [Route("/register")]
        public string Register(User user)
        {
            return string.Empty;
        }
    }
}
