using System.Net;
using MyHttpServer.Handlers;

namespace MyHttpServer;

public class Program
{
    private static void Main()
    {
        var listener = new HttpListener();
        var server = new ServerHandler(listener);
        server.Start();
        
    }
}