namespace StackUnderflow.Entities
{
    public class Vote
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public User User { get; set; }
    }
}