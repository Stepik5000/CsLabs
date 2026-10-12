using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class LogEntry
{
    public DateTime Timestamp;
    public string Level = string.Empty;
    public string Category = string.Empty;
    public string Message = string.Empty;
}

public class Program
{
    private const string TimestampFormat = "yyyy-MM-dd HH:mm:ss.fff";

    private const string ErrorLevel = "Error";
    private const string FatalLevel = "Fatal";
    private const string ServerCategory = "Server";

    private const string StatusOk = "Сервер работает штатно";
    private const string StatusErrors = "Есть ошибки: требуется проверка";
    private const string StatusCritical = "КРИТИЧЕСКАЯ ОШИБКА: сервер остановлен";

    public static void Main()
    {
        string[] lines = File.ReadAllLines("event_server.log");
        LogEntry[] entries = ParseLog(lines);

        Console.WriteLine(GetServerStatus(entries));
    }

    public static LogEntry[] ParseLog(string[] lines)
    {
        LogEntry[] entries = new LogEntry[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            entries[i] = ParseLine(lines[i]);
        }

        return entries;
    }

    public static LogEntry[] FilterByDate(LogEntry[] entries, DateTime date)
    {
        return Filter(entries, entry => entry.Timestamp.Date == date.Date);
    }

    public static LogEntry[] FilterByLevel(LogEntry[] entries, string level)
    {
        return Filter(entries, entry => entry.Level == level);
    }

    public static LogEntry[] FilterByCategory(LogEntry[] entries, string category)
    {
        return Filter(entries, entry => entry.Category == category);
    }

    public static LogEntry[] Search(LogEntry[] entries, string text)
    {
        return Filter(entries, entry =>
            entry.Message.Contains(text, StringComparison.OrdinalIgnoreCase));
    }

    public static int CountByLevel(LogEntry[] entries, string level)
    {
        return FilterByLevel(entries, level).Length;
    }

    public static string GetServerStatus(LogEntry[] entries)
    {
        if (Contains(entries, entry => entry.Level == FatalLevel && entry.Category == ServerCategory))
        {
            return StatusCritical;
        }

        if (Contains(entries, entry => entry.Level == ErrorLevel || entry.Level == FatalLevel))
        {
            return StatusErrors;
        }

        return StatusOk;
    }

    private static LogEntry ParseLine(string line)
    {
        LogEntry entry = new LogEntry();

        entry.Timestamp = DateTime.ParseExact(
            line.Substring(0, TimestampFormat.Length),
            TimestampFormat,
            CultureInfo.InvariantCulture);

        int firstOpenBracket = line.IndexOf('[');
        int firstCloseBracket = line.IndexOf(']', firstOpenBracket);
        int secondOpenBracket = line.IndexOf('[', firstCloseBracket);
        int secondCloseBracket = line.IndexOf(']', secondOpenBracket);

        entry.Level = line.Substring(firstOpenBracket + 1, firstCloseBracket - firstOpenBracket - 1);
        entry.Category = line.Substring(secondOpenBracket + 1, secondCloseBracket - secondOpenBracket - 1);
        entry.Message = line.Substring(secondCloseBracket + 2);

        return entry;
    }

    private static LogEntry[] Filter(LogEntry[] entries, Func<LogEntry, bool> condition)
    {
        List<LogEntry> result = new List<LogEntry>();

        for (int i = 0; i < entries.Length; i++)
        {
            if (condition(entries[i]))
            {
                result.Add(entries[i]);
            }
        }

        return result.ToArray();
    }

    private static bool Contains(LogEntry[] entries, Func<LogEntry, bool> condition)
    {
        for (int i = 0; i < entries.Length; i++)
        {
            if (condition(entries[i]))
            {
                return true;
            }
        }

        return false;
    }
}