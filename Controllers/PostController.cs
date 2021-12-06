using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using StackUnderflow.Entities;
using Microsoft.EntityFrameworkCore;
using JWT.Builder;
using JWT.Algorithms;
using System.Net;
using System.Web.Http;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Primitives;

namespace StackUnderflow.Controllers
{
    [ApiController]
    [Route("posts")]
    [EnableCors]
    public class PostController : ControllerBase
    {
        
        [HttpGet]
        public IActionResult GetPosts([FromHeader] string token)
        {
            var tokenValue = GetTokenValue(token);

            var usernameResult = GetUsernameFromJWT(tokenValue);
            if (usernameResult is BadRequestObjectResult)
                return usernameResult;

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
        public IActionResult GetPost([FromHeader] string token, int id)
        {
            var tokenValue = GetTokenValue(token);

            var usernameResult = GetUsernameFromJWT(tokenValue);
            if (usernameResult is BadRequestObjectResult)
                return usernameResult;

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
        public IActionResult CreatePost([FromHeader] string token, Post post)
        {
            var tokenValue = GetTokenValue(token);

            var usernameResult = GetUsernameFromJWT(tokenValue);
            if (usernameResult is BadRequestObjectResult)
                return usernameResult;
            var username = ((OkObjectResult)usernameResult).Value as string;

            using (var db = new StackUnderflowContext())
            {
                var user = db.Users.FirstOrDefault(user => user.Username == username);

                post.Creator = user;
                post.TimeStamp = DateTime.Now;

                db.Posts.Add(post);
                db.SaveChangesAsync();
            }

            return Ok(post);
        }

        [HttpGet]
        [Route("{id}/comments")]
        public IActionResult GetComments([FromHeader] string token, int id)
        {
            var tokenValue = GetTokenValue(token);

            var usernameResult = GetUsernameFromJWT(tokenValue);
            if (usernameResult is BadRequestObjectResult)
                return usernameResult;

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
        public IActionResult Comment([FromHeader] string token, int id, Comment comment)
        {
            var tokenValue = GetTokenValue(token);
            Post post;

            var usernameResult = GetUsernameFromJWT(tokenValue);
            if (usernameResult is BadRequestObjectResult)
                return usernameResult;
            var username = ((OkObjectResult)usernameResult).Value as string;

            using (var db = new StackUnderflowContext())
            {
                post = db.Posts.Include(post => post.Comments).ThenInclude(comment => comment.Creator).FirstOrDefault(post => post.Id == id);
                var user = db.Users.FirstOrDefault(user => user.Username == username);

                if (user == null || post == null)
                {
                    return Problem();
                }

                comment.TimeStamp = DateTime.Now;
                comment.Creator = user;

                post.Comments.Add(comment);
                db.SaveChangesAsync();
            }

            return Ok(post);
        }

        [HttpPost]
        [Route("{id}/vote")]
        public IActionResult Vote([FromHeader] string token, int id, Vote vote)
        {
            var tokenValue = GetTokenValue(token);

            var usernameResult = GetUsernameFromJWT(tokenValue);
            if (usernameResult is BadRequestObjectResult)
                return usernameResult;
            var username = ((OkObjectResult) usernameResult).Value as string;

            using (var db = new StackUnderflowContext())
            {
                var user = db.Users.FirstOrDefault(user => user.Username == username);
                var comment = db.Comments
                    .Include(c => c.Votes)
                    .FirstOrDefault(c => c.Id == id);

                vote.User = user;

                comment.Votes.Add(vote);
                db.SaveChangesAsync();
            }

            return Ok(vote);
        }

        [HttpGet]
        [Route("{id}/vote")]
        public IActionResult GetVotes([FromHeader] string token, int id)
        {
            var tokenValue = GetTokenValue(token);
            int result;

            var usernameResult = GetUsernameFromJWT(tokenValue);
            if (usernameResult is BadRequestObjectResult)
                return usernameResult;

            using (var db = new StackUnderflowContext())
            {
               
                var comment = db.Comments
                    .Include(c => c.Votes)
                    .FirstOrDefault(c => c.Id == id);

                result = comment.SumVotes();
            }

            return Ok(result);
        }


        [HttpGet]
        [Route("{id}/comments/accept/{commentId}")]
        public IActionResult AcceptComment([FromHeader] string token, int id, int commentId)
        {
            var tokenValue = GetTokenValue(token);

            var usernameResult = GetUsernameFromJWT(tokenValue);
            if (usernameResult is BadRequestObjectResult)
                return usernameResult;

            using (var db = new StackUnderflowContext())
            {
                var post = db.Posts
                    .Include(p => p.Comments)
                    .FirstOrDefault(p => p.Id == id);

                var comment = post.Comments
                    .FirstOrDefault(c => c.Id == commentId);
                
                post.AcceptedComment = comment;
                db.SaveChangesAsync();

                return Ok(comment);
            }
        }

        private IActionResult GetUsernameFromJWT(string token)
        {
            try
            {
                return Ok(JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                    .WithSecret("pdtxgsvrniydxeawhonytzxuysmhajff")
                    .MustVerifySignature()
                    .Decode(token));
            } catch (Exception ex)
            {
                return BadRequest(new RegistrationResponse()
                {
                    ErrorList = new List<string>()
                    {
                        "Invalid JWT"
                    },
                    Success = false
                });
            }
        }

        private string GetTokenValue(string token)
        {
            var regex = new Regex("{\\\"token\\\":\\\"([^\"]*)(?=\\\")");
            return regex.Match(token).Groups[1].Value;

        }
    }
}
