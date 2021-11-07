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
            using (var db = new StackUnderflowContext())
            {
                return new List<Post> {new() };
            }
            
        }

        [HttpGet]
        [Route("/{id}")]
        public IActionResult GetPost(int id)
        {
            using (var db = new StackUnderflowContext())
            {
                var post = db.Posts
                    .Where(post => post.Id == id)
                    .FirstOrDefault();

                if (post == null)
                {
                    return Problem();
                }

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(post);

                return Ok(json);
            }
        }

        [HttpPost]
        public Post CreatePost(Post post)
        {
            return new();
        }

        [HttpGet]
        [Route("/{id}/comments")]
        public Post GetComments(int id)
        {
            return new();
        }

        [HttpPost]
        [Route("/{id}/comment")]
        public Post Comment(int id, Comment comment)
        {
            return new();
        }

        [HttpPost]
        [Route("/{id}/vote")]
        public Post Vote(int id, Vote vote)
        {
            return new();
        }

        [HttpPost]
        [Route("/{id}/comments/accept")]
        public Post AcceptComment(int id, int commentId)
        {
            return new();
        }
    }
}
