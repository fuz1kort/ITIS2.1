using System.Net;

namespace MyHttpServer;

public class Program
{
    static void Main()
    {
        var _listener = new HttpListener();
        var server = new Server(_listener);
        server.Start();
    }
}