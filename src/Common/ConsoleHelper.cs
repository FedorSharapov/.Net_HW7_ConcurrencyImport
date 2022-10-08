using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Otus.Teaching.Concurrency.Import.Common
{
    public static class ConsoleHelper
    {
        static object _locker = new object();

        // usage: WriteLine("This is my [message] with inline [color] changes.", ConsoleColor.Yellow);
        public static void WriteLine(string message, ConsoleColor color = ConsoleColor.Yellow)
        {
            lock (_locker)
            {
                var pieces = Regex.Split(message, @"(\[[^\]]*\])");

                foreach (var piece in pieces)
                {
                    if (piece.StartsWith("[") && piece.EndsWith("]"))
                    {
                        Console.ForegroundColor = color;
                        Console.Write(piece.ToArray(), 1, piece.Length - 2);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                        Console.Write(piece);
                }

                Console.WriteLine();
            }
        }
        public static void WriteLineError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}