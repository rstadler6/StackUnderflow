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

                return Ok(posts);
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


                return Ok(post);
            }
        }

        [HttpPost]
        public IActionResult CreatePost(Post post, string username)
        {
            using (var db = new StackUnderflowContext())
            {
                
                post.TimeStamp = DateTime.Now;
                db.Posts.Add(post);
                db.SaveChangesAsync();
            }


            return Ok(post);
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


                return Ok(comments);
            }
        }

        [HttpPost]
        [Route("/{id}/comment")]
        public IActionResult Comment(int id, Comment comment, string username)
        {
            using (var db = new StackUnderflowContext())
            {
               
                comment.TiemeStamp = DateTime.Now;
                db.Comments.Add(comment);
                db.SaveChangesAsync();
            }

            return Ok(comment);
        }

        [HttpPost]
        [Route("/{id}/vote")]
        public IActionResult Vote(int id, Vote vote)
        {
            using (var db = new StackUnderflowContext())
            {
             
                var comment = db.Comments
                .Include(c => c.Votes)
                .Where(c => c.Id == id)
                .FirstOrDefault();

                comment.Votes.Add(vote);
                db.SaveChangesAsync();
            }

            return Ok(vote);
        }

        [HttpPost]
        [Route("/{id}/comments/accept")]
        public IActionResult AcceptComment(int id, int commentId)
        {
            using (var db = new StackUnderflowContext())
            {
                var posts = db.Posts
                    .Where(p => p.Id == id)
                    .Include(p => p.Comments)
                    .FirstOrDefault();
                
                var comment = posts.Comments
                    .Where(c => c.Id == commentId)
                    .FirstOrDefault();
                
                posts.AcceptedComment = comment;
                db.SaveChangesAsync();

                return Ok(comment);
            }

        }
    }
}
