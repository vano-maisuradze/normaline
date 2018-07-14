using System;

namespace Normalize
{
    public static class ConsoleMessage
    {
        public static void WriteInfo(string message)
        {
            LogMessage(message, ConsoleColor.White);
        }

        public static void WriteSuccess(string message)
        {
            LogMessage(message, ConsoleColor.Green);
        }

        public static void WriteError(string message)
        {
            LogMessage(message, ConsoleColor.Red);
        }

        private static void LogMessage(string message, ConsoleColor color)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = oldColor;
        }
    }
}
