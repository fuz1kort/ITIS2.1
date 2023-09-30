using System.Net;
using System.Text;
 
HttpListener server = new HttpListener();
// установка адресов прослушки
server.Prefixes.Add("http://127.0.0.1:2323/");
server.Start(); // начинаем прослушивать входящие подключения
Console.WriteLine("Сервер запущен");

while (true)
{
    // получаем контекст
    var context = await server.GetContextAsync();
    var response = context.Response;
    var request = context.Request;
    var filePath = "C:\\Users\\Marat\\source\\repos\\fuz1kort\\ITIS2.1\\MyHttpServer\\static\\battlenet.html";
    var fileContent = File.ReadAllText(filePath);
    Console.WriteLine("Запрос обработан");
// отправляемый в ответ код htmlвозвращает
    byte[] buffer = Encoding.UTF8.GetBytes(fileContent);
// получаем поток ответа и пишем в него ответ
    response.ContentLength64 = buffer.Length;
    await using Stream output = response.OutputStream;
// отправляем данные
    await output.WriteAsync(buffer);
    await output.FlushAsync();

    if (Console.ReadLine() == "stop")
        break;
}


 
Console.WriteLine("Запрос обработан");
Console.WriteLine("Сервер прекратил работу");
 
server.Stop();