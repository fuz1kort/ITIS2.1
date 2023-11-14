using System.Net;
using System.Text;
using MyHttpServer.Configuration;

namespace MyHttpServer.Handlers;

public class StaticFilesHandler : Handler
{
    public override async void HandleRequest(HttpListenerContext context)
    {
        var config = AppSettingsLoader.Instance();
        IsDirectoryExistAndCreate(config!.CurrentDirectory + config.Configuration!.StaticFilesPath);
        var currentStaticDirectory = config.CurrentDirectory + config.Configuration.StaticFilesPath;
        var response = context.Response;
        var request = context.Request;
        var requestUrl = request.Url?.LocalPath;

        if (requestUrl is "" or "/")
        {
            requestUrl = "/battlenet.html";
        }

        if (requestUrl!.Contains('.'))
        {
            var filePath = currentStaticDirectory + requestUrl;
            if (File.Exists(filePath))
            {
                var buffer = File.ReadAllBytes(filePath);
                response.ContentLength64 = buffer.Length;
                response.ContentType = GetContentType(requestUrl);
                await using Stream output = response.OutputStream;
                await output.WriteAsync(buffer);
                await output.FlushAsync();
            }

            else
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.ContentType = "text/plain; charset=utf-8";
                const string notFoundMessage = "404 Файл не найден";
                var notFoundBuffer = Encoding.UTF8.GetBytes(notFoundMessage);
                response.ContentLength64 = notFoundBuffer.Length;
                await using Stream output = response.OutputStream;
                await output.WriteAsync(notFoundBuffer);
                await output.FlushAsync();
                Console.WriteLine($"Файл не найден: {requestUrl}");
            }
        }

        else if (Successor != null)
        {
            Successor.HandleRequest(context);
        }
    }

    public static string GetContentType(string requestUrl)
    {
        string contentType;

        switch (Path.GetExtension(requestUrl).ToLower())
        {
            case ".html":
                contentType = "text/html; charset=utf-8";
                break;
            case ".png":
                contentType = "image/png";
                break;
            case ".svg":
                contentType = "image/svg+xml";
                break;
            case ".css":
                contentType = "text/css";
                break;
            default:
                contentType = "text/plain; charset=utf-8";
                break;
        }

        return contentType;
    }

    void IsDirectoryExistAndCreate(string path)
    {
        if (Directory.Exists(path))
            return;


        Directory.CreateDirectory(path);
    }
}