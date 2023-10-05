using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using MyHttpServer.Configuration;

class Handle
{
    private readonly HttpListener server;
    private readonly AppSettingsConfig config;
    public static string url = "http://127.0.0.1:1414/";
    public const string filePath = "C:/Users/Marat/source/repos/fuz1kort/ITIS2.1/MyHttpServer/static/battlenet.html";
    public static bool _running;

    public Handle()
    {
        server = new HttpListener();
        config = new AppSettingsConfig();
        _running = false;
    }

    public void Run()
    {
        server.Prefixes.Add(url);
        server.Start();
        Console.WriteLine("Сервер запущен: {0}", url);

        Task serverTask = HandleIncomingConnections();
        serverTask.GetAwaiter().GetResult();

        Console.WriteLine("Сервер прекратил работу");
        server.Stop();
    }

    public void Stop()
    {
        Console.WriteLine("Получена команда на остановку сервера.");
        server.Close();
        _running = false;
        Console.WriteLine("Сервер остановлен.");
    }

    public async Task HandleIncomingConnections()
    {


        while (_running)
        {
            var context = await server.GetContextAsync();
            var response = context.Response;

            var fileContent = await File.ReadAllTextAsync(filePath);


            var buffer = Encoding.UTF8.GetBytes(fileContent);

            response.ContentLength64 = buffer.Length;
            response.ContentType = "text/html";
            response.ContentEncoding = Encoding.UTF8;
            await using Stream output = response.OutputStream;

            await output.WriteAsync(buffer);
            await output.FlushAsync();
            response.Close();
        }
    }
}