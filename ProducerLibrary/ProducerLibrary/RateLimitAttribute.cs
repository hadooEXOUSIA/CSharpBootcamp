using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerLibrary
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]

    public class RateLimitAttribute : Attribute
    {
        public int Limit { get; }
        public RateLimitAttribute(int limit)
        {
            Limit = limit;
        }
    }
}
