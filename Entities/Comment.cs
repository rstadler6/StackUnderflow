using System.Collections.Generic;

namespace StackUnderflow.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public User Creator { get; set; }
        public List<Vote> Votes { get; set; }
    }
}