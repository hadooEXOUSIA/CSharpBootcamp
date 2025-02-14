using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerLibrary
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
