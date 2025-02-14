using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using MessageBroker.Services;

namespace MessageBroker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private static readonly object _lock = new object();
        private static Queue<Message> _messageQueue = new Queue<Message>();
        private const string MessageFilePath = "Storage\\messages.json";

        public MessagesController()
        {
            LoadMessagesFromFile();
        }

        [HttpPost]
        public IActionResult PostMessage([FromBody] Message message)
        {
            if (string.IsNullOrEmpty(message.Content) || string.IsNullOrEmpty(message.ProducerName))
            {
                LoggerService.Warning("Received an invalid message. Rejecting request.");
                return BadRequest(new { Message = "Message and ProducerName cannot be empty." });
            }

            lock (_lock)
            {
                _messageQueue.Enqueue(message);
                SaveMessagesToFile();
            }

            LoggerService.Info($"Message received from Producer {message.ProducerName}: {message.Content}");
            return Ok(new { Message = "Message received successfully!" });
        }

        [HttpGet]
        public IActionResult GetMessage([FromQuery] string producer)
        {
            if (_messageQueue.Count == 0)
            {
                LoggerService.Warning("No messages available.");
                return NotFound(new { Message = "No messages available." });
            }

            lock (_lock)
            {
                var message = string.IsNullOrEmpty(producer)
                    ? _messageQueue.Dequeue()
                    : _messageQueue.FirstOrDefault(m => m.ProducerName == producer);

                if (message == null)
                {
                    return NotFound(new { Message = $"No messages found for Producer: {producer}" });
                }


                _messageQueue = new Queue<Message>(_messageQueue.Where(m => m != message)); // Remove message
                SaveMessagesToFile();
                LoggerService.Info($"Message sent to Consumer {message.ProducerName}: {message.Content}");
                return Ok(message);
            }
        }

        private void LoadMessagesFromFile()
        {
            lock (_lock)
            {
                if (System.IO.File.Exists(MessageFilePath))
                {
                    try
                    {
                        var json = System.IO.File.ReadAllText(MessageFilePath);
                        _messageQueue = JsonSerializer.Deserialize<Queue<Message>>(json) ?? new Queue<Message>();
                    }
                    catch (JsonException)
                    {
                        _messageQueue = new Queue<Message>();
                        System.IO.File.WriteAllText(MessageFilePath, "[]");
                    }
                }
            }
        }

        private static void SaveMessagesToFile()
        {
            lock (_lock)
            {
                var json = JsonSerializer.Serialize(_messageQueue);
                System.IO.File.WriteAllText(MessageFilePath, json);
            }
        }
    }
}
