using System;

namespace ConsumerLibrary
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class RetryAttribute : Attribute
    {
        public int RetryCount { get; }
        public RetryAttribute(int retryCount) 
        {
            RetryCount = retryCount; 
        }
    }
}
