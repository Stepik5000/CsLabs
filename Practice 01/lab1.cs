namespace Practice_01;

class Program
{
    public static void Main()
    {
        Console.WriteLine("Name server");
        string Servername = Console.ReadLine();
        Console.WriteLine("quantity of players");
        int CountPlayers = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Loading Cpu");
        float cpuLoad = Convert.ToSingle(Console.ReadLine());
        Console.WriteLine("Ping");
        int ping = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("loading RAM (%):");
        float ramLoad = Convert.ToSingle(Console.ReadLine());
        Console.WriteLine("Is the server online? (True/False): ");
        bool isOnlineInput = Convert.ToBoolean(Console.ReadLine());
        Console.WriteLine("<<<<<<<Server's status>>>>>>>");
        Console.WriteLine($"<<<<<<<Name Server:{Servername}>>>>>>>");
        Console.WriteLine($"<<<<<<<Quantity of Players:{CountPlayers}>>>>>>>");
        Console.WriteLine($"<<<<<<<CPU Load:{cpuLoad}>>>>>>>");
        Console.WriteLine($"<<<<<<<Ping:{ping}>>>>>>>");
        Console.WriteLine($"<<<<<<<RAM Load:{ramLoad}>>>>>>>");
        Console.WriteLine($"<<<<<<<Is Online:{isOnlineInput}>>>>>>>");
    }
}



