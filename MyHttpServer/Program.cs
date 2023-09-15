using System.Net;
using System.Text;

HttpListener server = new HttpListener();
// установка адресов прослушки
server.Prefixes.Add("http://127.0.0.1:1235/");
server.Start(); // начинаем прослушивать входящие подключения

Console.WriteLine("Сервер был запущен");
while (true)
{

    var context = await server.GetContextAsync();

    var response = context.Response;
    byte[] buffer = File.ReadAllBytes(@"C:\Users\Marat\source\repos\fuz1kort\ITIS2.1\Google\index.html");
    // получаем поток ответа и пишем в него ответ
    response.ContentLength64 = buffer.Length;
    using Stream output = response.OutputStream;
    // отправляем данные
    await output.WriteAsync(buffer);
    await output.FlushAsync();
    Console.WriteLine("Запрос обработан");

    if (Console.ReadLine() == "stop")
    {
        server.Stop();
        break;
    }
    

    
}
