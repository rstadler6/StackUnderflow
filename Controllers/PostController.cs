using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackUnderflow.Entities;
using Microsoft.EntityFrameworkCore;

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
                    .Include(p => p.Comments)
                    .Where(p => p.Id == id)
                    .FirstOrDefault();

                var comments = posts.Comments;

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
        public IActionResult Vote(int id, Vote vote)
        {
            using (var db = new StackUnderflowContext())
            {
             
                
                db.Votes.Add(vote);
                db.SaveChangesAsync();
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(vote);

            return Ok(json);
        }

        [HttpPost]
        [Route("/{id}/comments/accept")]
        public IActionResult AcceptComment(int id, int commentId)
        {
            using (var db = new StackUnderflowContext())
            {
                var posts = db.Posts
                    .Include(p => p.Comments)
                    .Where(p => p.Id == id)
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
    }
}
