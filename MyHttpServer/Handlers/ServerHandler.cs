using System.Net;
using System.Text;
using MyHttpServer.Configuration;
using MyHttpServer.Handlers;

// using MyHttpServer.Configuration;

namespace MyHttpServer;

class ServerHandler
{
    private readonly HttpListener _server;
    private static bool _running;
    private readonly ServerData _serverData;
    private readonly AppSettingsConfig _config;

    public ServerHandler(HttpListener server, AppSettingsConfig config)
    {
        _server = server;
        _running = true;
        _config = config;
    }


    public void Stop()
    {
        Console.WriteLine("Получена команда на остановку сервера.");
        _server.Close();
        _running = false;
        Console.WriteLine("Сервер остановлен.");
    }

    public async Task Run()
    {
        while (_running)
        {
            try
            {
                var context = _server.GetContext();
                var response = context.Response;
                var request = context.Request;

                context = await _server.GetContextAsync();
                var staticFilesHandler = new StaticFilesHandler();
                var controllerHandler = new ControllerHandler();
                staticFilesHandler.Successor = controllerHandler;
                staticFilesHandler.HandleRequest(context);
                // var url = request.Url;
                // byte[] buffer = null;
                // var redirectedPath = url.LocalPath;
                //
                // if (redirectedPath == "" || redirectedPath == "/")
                //     redirectedPath = "/index.html";
                //
                // if (redirectedPath.Contains('.'))
                // {
                //     var filePath = _config.StaticFilesPath + redirectedPath;
                //     var result = ServerData.CheckIfFileExists(filePath)
                //         ? File.ReadAllBytes(filePath)
                //         : File.ReadAllBytes("NotFoundHtml.html");
                //
                //     buffer = result;
                //     var contentType = DetermineContentType(url);
                //     response.ContentType = $"{contentType}; charset=utf-8";
                //     response.ContentLength64 = buffer.Length;
                //     await using Stream output = response.OutputStream;
                //
                //     await output.WriteAsync(buffer);
                //     await output.FlushAsync();
                // }
                // // передача запроса дальше по цепи при наличии в ней обработчиков
                // else if (Successor != null)
                // {
                //     Successor.HandleRequest(context);
                // }
            }
            catch (HttpListenerException ex) when (ex.ErrorCode == 995)
            {
                break;
            }
        }
    }
    
    private string DetermineContentType(Uri url)
    {
        var stringUrl = url.ToString();
        var extension = "";
    
        try
        {
            extension = stringUrl.Substring(stringUrl.LastIndexOf('.'));
        }
        catch (Exception e)
        {
            extension = "text/html";
            return extension;
        }
        
        var contentType = "";
        switch (extension)
        {
            case ".htm":
            case ".html":
                contentType = "text/html";
                break;
            case ".css":
                contentType = "text/css";
                break;
            case ".js":
                contentType = "text/javascript";
                break;
            case ".jpg":
                contentType = "image/jpeg";
                break;
            case ".svg": 
            case ".xml":
                contentType = "image/" + "svg+xml";
                break;
            case ".jpeg":
            case ".png":
            case ".gif":
                contentType = "image/" + extension.Substring(1);
                // Console.WriteLine(extension.Substring(1));
                break;
            default:
                if (extension.Length > 1)
                {
                    contentType = "application/" + extension.Substring(1);
                }
                else
                {
                    contentType = "application/unknown";
                }
                break;
        }
    
    
        return contentType;
    }
}