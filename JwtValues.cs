using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StackUnderflow
{
    public class JwtValues
    {
        public DateTime ExpirationDate { get; set; }
        public string Username { get; set; }
    }
}
