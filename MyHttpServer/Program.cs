//using System.Net;
//using System.Text;


//HttpListener server = new HttpListener(); 
//// установка адресов прослушки
//server.Prefixes.Add("http://127.0.0.1:1235/");
//server.Start(); // начинаем прослушивать входящие подключения

//Console.WriteLine("Сервер был запущен");

//var serverThread = new Thread(ServerThread);
//serverThread.Start();
//void ServerThread()
//{
//    while (true)
//    {
//        var context = server.GetContext();


//        byte[] buffer = File.ReadAllBytes(@"C:\Users\gafar\Source\Repos\fuz1kort\ITIS2.1\Google\index.html");
//        // получаем поток ответа и пишем в него ответ
//        context.Response.ContentLength64 = buffer.Length;
//        context.Response.OutputStream.Write(buffer, 0, buffer.Length);
//        // отправляем данные
//        Console.WriteLine("Запрос обработан");

//        if (Console.ReadLine() == "stop")
//        {
//            server.Stop();
//            break;
//        }
//    }
//}

using System;
using System.IO;
using System.Net;
using System.Threading;


string address = "127.0.0.1";
int port = 1414;


var listener = new HttpListener();
listener.Prefixes.Add("http://" + address + ":" + port + "/");
listener.Start();
Console.WriteLine("Server started. Listening on " + address + ":" + port);

Thread serverThread = new Thread(ServerThread);
serverThread.Start();


Console.WriteLine("Press 'stop' to stop the server.");
while (true)
{
    if (Console.ReadLine() == "stop")
    {
        listener.Stop();
        Console.WriteLine("Server stopped.");
        break;
    }
}




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