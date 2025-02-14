using System;
using System.IO;

namespace MessageBroker.Services
{
    public class LoggerService
    {
        private static readonly string LogFilePath = "Storage\\logs.txt";
        private static readonly object _lock = new object(); // Prevent race conditions

        // Custom mothod to log messages
        private static void Log(string level, string message, ConsoleColor color)
        {
            lock (_lock) // Ensures thread safety for multiple log writes
            {
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
                Console.WriteLine(logEntry); // Print the logs on console
                System.IO.File.AppendAllText(LogFilePath, logEntry + Environment.NewLine); // Saves to file

            }
        }

        // Public methods for logging different levels
        public static void Info(string message) => Log("INFO", message, ConsoleColor.Green);
        public static void Warning(string message) => Log("WARNING", message, ConsoleColor.Yellow);
        public static void Error(string message) => Log("ERROR", message, ConsoleColor.Red);


    }
}
