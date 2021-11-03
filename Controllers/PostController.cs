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
    [Route("posts")]
    public class PostController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Post> GetPosts()
        {
            return new List<Post>{new()};
        }

        [HttpGet]
        public Post GetPost(int id)
        {
            return new();
        }

        [HttpPost]
        public Post CreatePost(Post post)
        {
            return new();
        }

        [HttpGet]
        public Post GetComments(int id)
        {
            return new();
        }

        [HttpPost]
        public Post Comment(int id, Comment comment)
        {
            return new();
        }

        [HttpPost]
        public Post Vote(int id, Vote vote)
        {
            return new();
        }

        [HttpPost]
        public Post AcceptComment(int id, int commentId)
        {
            return new();
        }
    }
}
