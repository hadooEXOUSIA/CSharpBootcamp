using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerLibrary
{
    internal interface IProducer
    {
        Task GenerateDataAsync(); // Method to generate and send data
    }
}
