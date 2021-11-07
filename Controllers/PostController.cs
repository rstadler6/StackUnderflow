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
        public ActionResult<Post> GetPosts()
        {
            using (var db = new StackUnderflowContext())
            {
                var posts = db.Posts;


                if (posts == null)
                {
                    return Problem();
                }

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(posts);

                return Ok(json);
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
        public IActionResult CreatePost(Post post, string username)
        {
            using (var db = new StackUnderflowContext())
            {
                var userObj = db.Users
                    .Where(u => u.Username == username)
                    .FirstOrDefault();

                if (userObj == null)
                {
                    return Problem();
                }

                post.TimeStamp = DateTime.Now;
                db.Posts.Add(post);
                db.SaveChangesAsync();
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(post);

            return Ok(json);
        }

        [HttpGet]
        [Route("/{id}/comments")]
        public ActionResult<Comment> GetComments(int id)
        {
            using (var db = new StackUnderflowContext())
            {
                var posts = db.Posts
                    .Where(posts => posts.Id == id);

                var comments = posts


                if (comments == null)
                {
                    return Problem();
                }

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(comments);

                return Ok(json);
            }
        }

        [HttpPost]
        [Route("/{id}/comment")]
        public IActionResult Comment(int id, Comment comment, string username)
        {
            using (var db = new StackUnderflowContext())
            {
                var userObj = db.Users
                    .Where(u => u.Username == username)
                    .FirstOrDefault();

                if (userObj == null)
                {
                    return Problem();
                }

                comment.TiemeStamp = DateTime.Now;
                db.Comments.Add(comment);
                db.SaveChangesAsync();
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(comment);

            return Ok(json);
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
