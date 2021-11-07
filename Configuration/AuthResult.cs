using System.Collections.Generic;

namespace StackUnderflow.Configuration
{
    public class AuthResult
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public List<string> ErrorList { get; set; }

    }
}