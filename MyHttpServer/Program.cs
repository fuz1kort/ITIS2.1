using System.Net;
using MyHttpServer.Configuration;

namespace MyHttpServer;

public class Program
{
    static void Main()
    {
        var config = AppSettingsConfig.LoadServerConfig();
        if (config == null)
        {
            Console.WriteLine("Ошибка при загрузке конфигурации. Сервер не может быть запущен.");
            return;
        }

        var server = new HttpListener();
        var prefix = $"{config.Address}:{config.Port}/";
        server.Prefixes.Add(prefix);
        server.Start();
        Console.WriteLine("Сервер запущен: {0}", prefix);

        var handle = new Handle(server, config);
        
        
        
        Task.Run(() =>
        {
            while (true)
            {
                string consoleInput = Console.ReadLine();
                if (consoleInput == "stop")
                {
                    handle.Stop();
                    break;
                }
            }
        });
        handle.Run();
    }
}