using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks; 

namespace ProducerApp
{
    
    class ProducerApp
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("\n ____                _                          _                \r\n|  _ \\ _ __ ___   __| |_   _  ___ ___ _ __     / \\   _ __  _ __  \r\n| |_) | '__/ _ \\ / _` | | | |/ __/ _ \\ '__|   / _ \\ | '_ \\| '_ \\ \r\n|  __/| | | (_) | (_| | |_| | (_|  __/ |     / ___ \\| |_) | |_) |\r\n|_|   |_|  \\___/ \\__,_|\\__,_|\\___\\___|_|    /_/   \\_\\ .__/| .__/ \r\n                                                    |_|   |_|    \n");
            string apiUrl = "http://localhost:5052/api/messages"; // Web API URL
            string dllPath = "Addons\\ProducerLibrary.dll";
            // Ensure DLL is placed in Addons folder

            if (!System.IO.File.Exists(dllPath))
            {
                Console.WriteLine("❌ ProducerLibrary.dll not found in Addons folder.");
                return;
            }

            var assembly = Assembly.LoadFrom(dllPath);
            var producerType = assembly.GetTypes().FirstOrDefault(t => t.GetInterface("IProducer") != null);

            if (producerType == null)
            {
                Console.WriteLine("❌ No valid producer found in DLL.");
                return;
            }

            var producer = (dynamic)Activator.CreateInstance(producerType, apiUrl);
            Console.WriteLine($"✅ Loaded Producer: {producerType.Name}");

            await producer.GenerateDataAsync(); // Calls method dynamically
        }
    }

}
