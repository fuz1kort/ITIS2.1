using System.Net;
using MyHttpServer.Configuration;
using MyHttpServer.Controllers;

namespace MyHttpServer.Handlers;

public class ServerHandler
{
    public readonly HttpListener Server;
    private bool _isRunning;
    public readonly string Prefix;
    private readonly AppSettingsConfig? _config;
    private Handler _staticFileHandler = new StaticFilesHandler();
    private Handler? _controllerHandler = new ControllerHandler();

    public ServerHandler(HttpListener server)
    {
        Server = server;
        _isRunning = false;
        var cfgLoader = new AppSettingsLoader();
        cfgLoader.InitAppSettings();
        _config = cfgLoader.Configuration;
        Prefix = $"{_config!.Address}:{_config!.Port}/";
    }

    public async Task Start()
    {
        _isRunning = true;
        Server.Prefixes.Add(Prefix);
        Server.Start();


        try
        {
            Server.Start();
            Console.WriteLine($"Сервер запущен: {Prefix}");
            var stopThread = new Thread(() =>
            {
                while (_isRunning)
                {
                    if (Console.ReadLine() == "stop")
                    {
                        _isRunning = false;
                        Console.WriteLine("Сервер остановлен");
                        Server.Stop();
                    }
                        
                }
            });
            stopThread.Start();

            if (!CheckIfStaticFolderExists(_config?.StaticFilesPath!))
                Directory.CreateDirectory(_config?.StaticFilesPath!);
        }
        
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        while (_isRunning)
        {
            var context = await Server.GetContextAsync();
            _staticFileHandler.Successor = _controllerHandler;
            _staticFileHandler.HandleRequest(context);
        }
    }

    private bool CheckIfStaticFolderExists(string staticFolderPath)
    {
        return Directory.Exists(staticFolderPath);
    }
}