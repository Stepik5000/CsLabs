using System;
using System.Globalization;

namespace ConsoleApp1
{
    internal class Program
    {
        public static DateTime GetLogDateTime(string line)
        {
            int index1 = line.IndexOf(' ');
            int index2 = line.IndexOf(' ', index1 + 1);

            string datetimeStr = line.Substring(0, index2);

            return DateTime.ParseExact(
                datetimeStr,
                "yyyy-MM-dd HH:mm:ss.fff",
                CultureInfo.InvariantCulture
            );
        }

        public static string GetLogLevel(string line)
        {
            int start = line.IndexOf('[') + 1;
            int end = line.IndexOf(']');

            return line.Substring(start, end - start);
        }

        public static string GetLogType(string line)
        {
            int firstBracket = line.IndexOf('[');
            int start = line.IndexOf('[', firstBracket + 1) + 1;
            int end = line.IndexOf(']', start);

            return line.Substring(start, end - start);
        }

        public static string GetLogText(string line)
        {
            int firstBracket = line.IndexOf(']');
            int secondBracket = line.IndexOf(']', firstBracket + 1);

            return line.Substring(secondBracket + 2);
        }

        public static (DateTime, string, string, string) GetLogParts(string line)
        {
            DateTime dateTime = GetLogDateTime(line);
            string level = GetLogLevel(line);
            string type = GetLogType(line);
            string text = GetLogText(line);

            return (dateTime, level, type, text);
        }

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("event_server.log");

            foreach (string line in lines)
            {
                var parts = GetLogParts(line);

                Console.WriteLine($"Date: {parts.Item1}");
                Console.WriteLine($"Level: {parts.Item2}");
                Console.WriteLine($"Type: {parts.Item3}");
                Console.WriteLine($"Text: {parts.Item4}");

                Console.WriteLine(new string('-', 60));
            }
        }
    }
}