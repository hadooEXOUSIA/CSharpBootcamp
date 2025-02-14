using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumerLibrary
{
    internal interface IConsumer
    {
        Task RetrieveDataAsync(); // Method to receive messages from API

    }
}
