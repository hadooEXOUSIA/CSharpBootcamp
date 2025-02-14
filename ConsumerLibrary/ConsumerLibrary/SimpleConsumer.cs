using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ConsumerLibrary
{
    [RateLimit(5)] // Creates 5 parallel threads
    [Retry(3)] // Retries up to 3 times if message retrieval fails
    public class SimpleConsumer : IConsumer
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly int _retryCount;
        private readonly int _threadCount;

        public SimpleConsumer(string apiUrl, string producerName)
        {
            _httpClient = new HttpClient();
            _apiUrl = $"{apiUrl}?producer={producerName}";

            // Read RateLimit and Retry attributes dynamically
            var retryAttr = (RetryAttribute)Attribute.GetCustomAttribute(typeof(SimpleConsumer), typeof(RetryAttribute));
            _retryCount = retryAttr?.RetryCount ?? 3; // Default to 3 retries

            var rateLimitAttr = (RateLimitAttribute)Attribute.GetCustomAttribute(typeof(SimpleConsumer), typeof(RateLimitAttribute));
            _threadCount = rateLimitAttr?.Limit ?? 1; // Default to 1 thread
        }

        public async Task RetrieveDataAsync()
        {
            Console.WriteLine($"Consumer started with {_threadCount} threads and {_retryCount} retry...");

            // Create and start multiple threads
            Task[] threads = new Task[_threadCount];

            for (int i = 0; i < _threadCount; i++)
            {
                int threadId = i + 1;
                threads[i] = Task.Run(async () =>
                {
                    while (true) // Each thread continuously retrieves data
                    {
                        bool success = await RetrieveMessageAsync(threadId);

                        if (!success)
                        {
                            Console.WriteLine($"❌ Message Broker API is unavailable. Consumer (Thread-{threadId}) is stopping.");
                            break; // Stop execution if API is down
                        }

                        Thread.Sleep(2000); // Each thread waits 2 seconds before the next request
                    }
                });
            }

            // Wait for all threads to complete execution
            await Task.WhenAll(threads);
        }

        private async Task<bool> RetrieveMessageAsync(int threadId)
        {
            for (int i = 0; i < _retryCount; i++) // Retry up to _retryCount times
            {
                try
                {
                    HttpResponseMessage response = await _httpClient.GetAsync(_apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"✅ [Thread-{threadId}] Received Message: {json}");
                        return true; // Message received successfully
                    }
                    else
                    {
                        Console.WriteLine($"⚠️ [Thread-{threadId}] Failed to retrieve message. Retrying... Attempt {i + 1}/{_retryCount}");
                        await Task.Delay(1000); // Wait 1 second before retrying
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"❌ Error retrieving message: {ex.Message}");
                    Console.WriteLine($"⚠️ Retrying... Attempt {i + 1}/{_retryCount}");
                    await Task.Delay(1000); // Wait 1 second before retrying
                }
            }

            return false; // Return false after all retries fail
        }
    }
}
