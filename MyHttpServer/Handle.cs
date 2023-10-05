using System.Net;
using System.Text;
using MyHttpServer.Configuration;

// using MyHttpServer.Configuration;

namespace MyHttpServer;

class Handle
{
    private readonly HttpListener _server;
    private static bool _running;
    private AppSettingsConfig _config;

    public Handle(HttpListener server, AppSettingsConfig config)
    {
        _server = server;
        _config = config;
        _running = true;
    }


    public void Stop()
    {
        Console.WriteLine("Получена команда на остановку сервера.");
        _server.Close();
        _running = false;
        Console.WriteLine("Сервер остановлен.");
    }

    public void Run()
    {
        while (_running)
        {
            try
            {
                var context = _server.GetContext();
                var response = context.Response;
                var request = context.Request;

                var requestUrl = request.Url.LocalPath;
                if (requestUrl.EndsWith(".html") || requestUrl.EndsWith('/'))
                {
                    var filePath = Path.Combine(_config.StaticFilesPath, requestUrl.TrimStart('/'));
                    Console.WriteLine(filePath);
                    if (filePath.EndsWith("static"))
                    {
                        response.ContentType = "text/html;";
                        var buffer = File.ReadAllBytes(Path.Combine(_config.StaticFilesPath, "index.html"));
                        response.ContentLength64 = buffer.Length;
                        response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                    }
                    
                    else if (filePath.EndsWith(".css"))
                    {
                        response.ContentType = "text/css";
                        var buffer = File.ReadAllBytes(filePath);
                        response.ContentLength64 = buffer.Length;
                        response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                    }
                    
                    
                    else if (filePath.EndsWith(".jpg") || filePath.EndsWith(".jpeg"))
                    {
                        response.ContentType = "image/jpeg";
                        var buffer = File.ReadAllBytes(filePath);
                        response.ContentLength64 = buffer.Length;
                        response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                    }
                    
                    else if (filePath.EndsWith(".png"))
                    {
                        response.ContentType = "image/png";
                        var buffer = File.ReadAllBytes(filePath);
                        response.ContentLength64 = buffer.Length;
                        response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                    }
                    
                    else if (filePath.EndsWith(".svg"))
                    {
                        response.ContentType = "image/svg+xml";
                        var buffer = File.ReadAllBytes(filePath);
                        response.ContentLength64 = buffer.Length;
                        response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                    }
                    
                    else if (filePath.EndsWith(".ico"))
                    {
                        response.ContentType = "image/x-icon";
                        var buffer = File.ReadAllBytes(filePath);
                        response.ContentLength64 = buffer.Length;
                        response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                    }
                    
                    else
                    {
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        response.ContentType = "text/plain; charset=utf-8";
                        var notFoundMessage = "404 File Not Found - файл не найден";
                        var buffer = Encoding.UTF8.GetBytes(notFoundMessage);
                        response.ContentLength64 = buffer.Length;
                        response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                        Console.WriteLine("File not founded");
                    }
                }
                response.Close();
            }
            catch (HttpListenerException ex) when (ex.ErrorCode == 995)
            {
                break;
            }
        }
    }
}