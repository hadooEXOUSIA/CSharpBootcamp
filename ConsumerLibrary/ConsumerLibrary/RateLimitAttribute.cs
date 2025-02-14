using System;

namespace ConsumerLibrary
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
