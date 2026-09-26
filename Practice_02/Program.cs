namespace Practice_02;

public class Program
{
    public static void Main()
    {
        
        string status = CheckConfiguration(50, 8, true, false);
        Console.WriteLine(status);
    }

    public static string CheckConfiguration(int maxPlayers, int ramGb, bool isPublic, bool hasPassword)
    {
        
        if (maxPlayers <= 0)
        {
            return "Запуск невозможен: количество игроков должно быть больше нуля.";
        }

        if (ramGb < 2)
        {
            return "Запуск невозможен: серверу недостаточно оперативной памяти.";
        }

        
        if (isPublic && hasPassword)
        {
            return "Запуск возможен с предупреждением: публичный сервер защищён паролем.";
        }

        if (maxPlayers > 100 && ramGb < 8)
        {
            return "Запуск возможен с предупреждением: для такого количества игроков рекомендуется больше оперативной памяти.";
        }

        
        return "Сервер готов к запуску.";
    }
}