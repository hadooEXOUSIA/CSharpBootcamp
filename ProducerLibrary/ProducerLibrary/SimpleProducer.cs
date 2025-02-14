using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerLibrary
{
    [RateLimit(5)] // Creates 5 parallel threads
    [Retry(3)] // Retries up to 3 times if message sending fails
    public class SimpleProducer : IProducer
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly int _retryCount;
        private readonly int _threadCount;
        private readonly string _producerName;

        public SimpleProducer(string apiUrl)
        {
            _httpClient = new HttpClient();
            _apiUrl = apiUrl;

            // Assign a unique producer name
            Console.Write("Enter Producer Name: ");
            _producerName = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(_producerName))
                _producerName = $"Producer-{Guid.NewGuid()}"; // Default unique name - producer name cannot be empty

            var retryAttr = (RetryAttribute)Attribute.GetCustomAttribute(typeof(SimpleProducer), typeof(RetryAttribute));
            _retryCount = retryAttr?.RetryCount ?? 3;

            var rateLimitAttr = (RateLimitAttribute)Attribute.GetCustomAttribute(typeof(SimpleProducer), typeof(RateLimitAttribute));
            _threadCount = rateLimitAttr?.Limit ?? 1;
        }

        public async Task GenerateDataAsync()
        {
            Console.WriteLine($"Producer {_producerName} started with {_threadCount} threads and {_retryCount} retry");

            Task[] threads = new Task[_threadCount];

            for (int i = 0; i < _threadCount; i++)
            {
                int threadId = i + 1;
                threads[i] = Task.Run(async () =>
                {
                    int counter = 1;
                    while (true) // Producing meassages indefinitely to keep trying the code 
                    {
                        var message = new // Example of meassge form 
                        {
                            ProducerName = _producerName,
                            Content = $"[Thread-{threadId}] Generated Message {counter}"
                        };

                        bool success = await SendMessageAsync(message);

                        if (!success)
                        {
                            Console.WriteLine($"❌ Message Broker API is unavailable. Producer (Thread-{threadId}) is stopping.");
                            break;
                        }

                        counter++;
                        Thread.Sleep(2000); // 2 seconds wait for every message generation
                    }
                });
            }
            // Wait for all tasks to be finished 
            await Task.WhenAll(threads);
        }

        // Sending message method 
        private async Task<bool> SendMessageAsync(object message)
        {
            for (int i = 0; i < _retryCount; i++)
            {
                try 
                {
                    var jsonMessage = JsonSerializer.Serialize(message);
                    var content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _httpClient.PostAsync(_apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"✅ Message sent successfully: {jsonMessage}");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"⚠️ Failed to send message. Retrying... Attempt {i + 1}/{_retryCount}");
                        await Task.Delay(1000);
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"❌ Error sending message: {ex.Message}");
                    Console.WriteLine($"⚠️ Retrying... Attempt {i + 1}/{_retryCount}");
                    await Task.Delay(1000);
                }
            }

            return false;
        }
    }
}
