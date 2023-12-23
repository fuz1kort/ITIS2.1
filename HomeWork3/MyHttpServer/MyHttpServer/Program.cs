using MyHttpServer.Configuration;
using System.Net;
using System.Text.Json;

const string pathConfigFile = "../../../appsetting.json";
HttpListener listener = new();

try
{
    if (!File.Exists(pathConfigFile))
    { 
        Console.WriteLine("Файл appsetting.json не был найден");
        throw new Exception();
    }

    AppSettings? config;

    using (var file = File.OpenRead(pathConfigFile)) 
        config = JsonSerializer.Deserialize<AppSettings>(file);

    listener.Prefixes.Add(config!.Address + ":" + config.Port + "/");
    listener.Start();
    Console.WriteLine("Server started");

    Task.Run(ServerThread);

    Console.WriteLine("Press 'stop' to stop the server.");
    Console.ReadLine();
    listener.Stop();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
finally
{
    Console.WriteLine("Работа сервера завершена");
}

return;

void ServerThread()
{
    while (listener.IsListening)
    {
        var context = listener.GetContext();
        var response = context.Response;
        const string filePath = "../../../index.html";
        var buffer = File.ReadAllBytes(filePath);
        using var output = response.OutputStream;
        output.Write(buffer);
        output.Flush();
    }
}