using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

class ConsumerApp
{
    static async Task Main(string[] args)
    {
        string apiUrl = "http://localhost:5052/api/messages"; // Message Broker API URL
        string dllPath = "Addons/ConsumerLibrary.dll"; // Ensure DLL is placed in Addons folder
        Console.WriteLine("\n  ____                                               _                \r\n / ___|___  _ __  ___ _   _ _ __ ___   ___ _ __     / \\   _ __  _ __  \r\n| |   / _ \\| '_ \\/ __| | | | '_ ` _ \\ / _ \\ '__|   / _ \\ | '_ \\| '_ \\ \r\n| |__| (_) | | | \\__ \\ |_| | | | | | |  __/ |     / ___ \\| |_) | |_) |\r\n \\____\\___/|_| |_|___/\\__,_|_| |_| |_|\\___|_|    /_/   \\_\\ .__/| .__/ \r\n                                                         |_|   |_|    \n");
        if (!File.Exists(dllPath))
        {
            Console.WriteLine("❌ ConsumerLibrary.dll not found in Addons folder.");
            return;
        }

        var assembly = Assembly.LoadFrom(dllPath);
        var consumerType = assembly.GetTypes().FirstOrDefault(t => t.GetInterfaces().Any(i => i.Name == "IConsumer"));

        if (consumerType == null)
        {
            Console.WriteLine("❌ No valid consumer found in DLL.");
            return;
        }

        // Ask user which Producer to subscribe to
        Console.Write("Enter Producer Name to Subscribe: ");
        string producerName = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(producerName))
            producerName = "default";

        // Dynamically create an instance of the consumer
        object consumerInstance = Activator.CreateInstance(consumerType, new object[] { apiUrl, producerName });

        // Get the RetrieveDataAsync method using reflection
        MethodInfo retrieveDataMethod = consumerType.GetMethod("RetrieveDataAsync");

        if (retrieveDataMethod != null)
        {
            // Invoke RetrieveDataAsync dynamically
            Task retrieveTask = (Task)retrieveDataMethod.Invoke(consumerInstance, null);
            await retrieveTask;
        }
        else
        {
            Console.WriteLine("❌ Error: Could not find RetrieveDataAsync method in the consumer.");
        }
    }
}
