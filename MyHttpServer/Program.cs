namespace MyHttpServer;

public class Program
{
    public static void Main()
    {
        var server = new Handle();
        server.Run();
        Task.Run(() =>
        {
            while (true)
            {
                string consoleInput = Console.ReadLine();
                if (consoleInput == "stop")
                {
                    server.Stop();
                    break;
                }
            }
        });
    }

}