using MyHttpServer.Configuration;
using System.Net;
using System.Text.Json;

const string pathConfigFile = @".\appsetting1.json";
HttpListener listener = new();

try
{
    if (!File.Exists(pathConfigFile))
    { 
        Console.WriteLine("Файл appsetting.json не был найден");
        throw new Exception();
        return;
    }

    AppSettings config;

    using (var file = File.OpenRead(pathConfigFile))
    {
        config = JsonSerializer.Deserialize<AppSettings>(file);

    }

    listener.Prefixes.Add("http://" + config.Address + ":" + config.Port + "/");
    listener.Start();
    Console.WriteLine("Server started");

    Thread serverThread = new Thread(ServerThread);
    serverThread.Start();


    Console.WriteLine("Press 'stop' to stop the server.");
    Console.ReadLine();
    listener.Stop();
}
catch(Exception ex)
{

}
finally
{
    Console.WriteLine("Работа сервера завершена");
}

//while (true)
//{
//    if (Console.ReadLine() == "stop")
//    {
//        listener.Stop();
//        Console.WriteLine("Server stopped.");
//        break;
//    }
//}




void ServerThread()
{
    while (listener.IsListening)
    {
        HttpListenerContext context = listener.GetContext();
        string filePath = @"C:\Users\gafar\Source\Repos\fuz1kort\ITIS2.1\Google\index.html";
        if (File.Exists(filePath))
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
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
