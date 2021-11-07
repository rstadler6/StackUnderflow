using System;
using System.Collections.Generic;

namespace StackUnderflow.Entities
{
    public class Post
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public User Creator { get; set; }
        public DateTime TimeStamp { get; set; }
        public List<Comment> Comments { get; set; }
        public Comment AcceptedComment { get; set; }
    }
}