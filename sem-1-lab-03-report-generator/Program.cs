using System;
using System.Globalization;
using System.IO;
using System.Text;

public class Program
{
    private const string EventName = "Восстание Ледяного Пламени";
    private const string StartMarker = "Событие началось: " + EventName;
    private const string EndMarker = "Событие \"" + EventName + "\" закрыто";

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        string[] lines = File.ReadAllLines("event_server.log");
        Console.WriteLine(BuildReport(lines));
    }


    public static string BuildReport(string[] lines)
    {
        string[] eventLines = GetEventLines(lines);

        DateTime date = GetEventDate(eventLines);
        string winner = GetWinner(eventLines) ?? "не определён";
        int winnerPoints = GetWinnerPoints(eventLines, winner);
        string item = GetEventItem(eventLines) ?? "нет";
        int consolationPoints = GetConsolationPoints(eventLines);
        int warnings = CountByLevel(eventLines, "Warning");
        int errors = CountByLevel(eventLines, "Error");

        string report = "# Итоги события: " + EventName + "\n\n";
        report += "Дата: " + date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture) + "\n";
        report += "Победитель: " + winner + "\n";
        report += "Очки победителя: " + winnerPoints + "\n";
        report += "Ивентовый предмет: " + item + "\n";
        report += "Утешительная награда Железных волков: " + consolationPoints + " очков\n";
        report += "Предупреждений во время события: " + warnings + "\n";
        report += "Ошибок во время события: " + errors;

        return report;
    }


    private static int FindLine(string[] lines, string text)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains(text))
            {
                return i;
            }
        }
        return -1;
    }


    private static string[] GetEventLines(string[] lines)
    {
        int start = FindLine(lines, StartMarker);
        if (start == -1)
        {
            return new string[0];
        }

        int end = lines.Length - 1; 
        for (int i = start; i < lines.Length; i++)
        {
            if (lines[i].Contains(EndMarker))
            {
                end = i;
                break;
            }
        }

        string[] result = new string[end - start + 1];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = lines[start + i];
        }
        return result;
    }

    private static string GetMessage(string line)
    {
        return line.Substring(line.IndexOf("] ") + 2);
    }

    private static DateTime GetEventDate(string[] eventLines)
    {
        int index = FindLine(eventLines, "Событие началось:");
        if (index == -1)
        {
            return DateTime.MinValue;
        }

        string timestamp = eventLines[index].Substring(0, 23); 
        return DateTime.ParseExact(timestamp, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
    }

    private static string? GetWinner(string[] eventLines)
    {
        const string ending = " объявлены победителями";
        foreach (string line in eventLines)
        {
            if (line.Contains("[Reward]") && line.Contains(ending))
            {
                string message = GetMessage(line);
                return message.Substring(0, message.IndexOf(ending));
            }
        }
        return null;
    }

    private static int GetWinnerPoints(string[] eventLines, string winner)
    {
        string startText = winner + " получили ";
        foreach (string line in eventLines)
        {
            if (line.Contains("[Reward]") && line.Contains(startText)
                && line.Contains(" очков события") && !line.Contains("утешительную"))
            {
                string message = GetMessage(line);
                int start = message.IndexOf(startText) + startText.Length;
                int end = message.IndexOf(" очков", start);
                return int.Parse(message.Substring(start, end - start));
            }
        }
        return 0;
    }

    private static string? GetEventItem(string[] eventLines)
    {
        const string marker = "получили ивентовый предмет: ";
        foreach (string line in eventLines)
        {
            if (line.Contains("[Loot]") && line.Contains(marker))
            {
                return line.Substring(line.IndexOf(marker) + marker.Length).Trim();
            }
        }
        return null;
    }

    private static int GetConsolationPoints(string[] eventLines)
    {
        const string marker = "утешительную награду: ";
        foreach (string line in eventLines)
        {
            if (line.Contains("[Reward]") && line.Contains(marker))
            {
                int start = line.IndexOf(marker) + marker.Length;
                int end = line.IndexOf(" очков", start);
                return int.Parse(line.Substring(start, end - start));
            }
        }
        return 0;
    }
    private static int CountByLevel(string[] eventLines, string level)
    {
        string tag = "[" + level + "]";
        int count = 0;
        foreach (string line in eventLines)
        {
            if (line.Contains(tag))
            {
                count++;
            }
        }
        return count;
    }
}