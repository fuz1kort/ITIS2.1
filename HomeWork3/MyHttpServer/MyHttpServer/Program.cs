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

    var serverThread = new Thread(ServerThread);
    serverThread.Start();


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
        const string filePath = "../../../index.html";
        if (File.Exists(filePath))
        {
            var fileBytes = File.ReadAllBytes(filePath);
            context.Response.ContentLength64 = fileBytes.Length;
            context.Response.OutputStream.Write(fileBytes, 0, fileBytes.Length);
        }
        else
        {
            context.Response.StatusCode = 404;
        }
        context.Response.OutputStream.Close();
    }
}