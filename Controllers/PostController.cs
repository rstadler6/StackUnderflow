using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using StackUnderflow.Entities;
using Microsoft.EntityFrameworkCore;

namespace StackUnderflow.Controllers
{
    [ApiController]
    [Route("posts")]
    [EnableCors("CorsPolicy")]
    public class PostController : ControllerBase
    {
        [HttpGet]
        public ActionResult<Post> GetPosts()
        {
            using (var db = new StackUnderflowContext())
            {
                var posts = db.Posts
                    .Include(post => post.Comments).ThenInclude(comment => comment.Creator)
                    .Include(post => post.Creator).ToList();


                if (posts.Count == 0)
                {
                    return Problem();
                }

                return Ok(posts);
            }
            
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetPost(int id)
        {
            using (var db = new StackUnderflowContext())
            {
                var post = db.Posts
                    .Include(post => post.Creator)
                    .Include(post => post.Comments)
                    .FirstOrDefault(post => post.Id == id);

                if (post == null)
                {
                    return Problem();
                }


                return Ok(post);
            }
        }

        [HttpPost]
        public IActionResult CreatePost(Post post)
        {
            using (var db = new StackUnderflowContext())
            {
                var user = db.Users.FirstOrDefault(user => user.Id == 1);

                post.Creator = user;
                post.TimeStamp = DateTime.Now;

                db.Posts.Add(post);
                db.SaveChangesAsync();
            }

            return Ok(post);
        }

        [HttpGet]
        [Route("{id}/comments")]
        public ActionResult<Comment> GetComments(int id)
        {
            using (var db = new StackUnderflowContext())
            {
                var posts = db.Posts
                    .Include(p => p.Comments)
                    .FirstOrDefault(p => p.Id == id);

                var comments = posts.Comments;

                if (comments == null)
                {
                    return Problem();
                }


                return Ok(comments);
            }
        }

        [HttpPost]
        [Route("{id}/comment")]
        public IActionResult Comment(int id, Comment comment)
        {
            using (var db = new StackUnderflowContext())
            {
                var post = db.Posts.Include(post => post.Comments).FirstOrDefault(post => post.Id == id);
                var user = db.Users.FirstOrDefault(user => user.Id == 1);//user => user.Username == HttpContext.User.Claims.First().Value);

                if (user == null || post == null)
                {
                    return Problem();
                }

                comment.TimeStamp = DateTime.Now;
                comment.Creator = user;

                post.Comments.Add(comment);
                db.SaveChangesAsync();
            }

            return Ok(comment);
        }

        [HttpPost]
        [Route("{id}/vote")]
        public IActionResult Vote(int id, Vote vote)
        {
            using (var db = new StackUnderflowContext())
            {
                var user = db.Users.FirstOrDefault(user => user.Id == 1);
                var comment = db.Comments
                    .Include(c => c.Votes)
                    .FirstOrDefault(c => c.Id == id);

                vote.User = user;

                comment.Votes.Add(vote);
                db.SaveChangesAsync();
            }

            return Ok(vote);
        }

        [HttpPost]
        [Route("{id}/comments/accept")]
        public IActionResult AcceptComment(int id, int commentId)
        {
            using (var db = new StackUnderflowContext())
            {
                var posts = db.Posts
                    .Include(p => p.Comments)
                    .FirstOrDefault(p => p.Id == id);

                var comment = posts.Comments
                    .FirstOrDefault(c => c.Id == commentId);
                
                posts.AcceptedComment = comment;
                db.SaveChangesAsync();

                return Ok(comment);
            }

        }
    }
}
