using MyHttpServer;

/*
    1) batlle net должен содержать папки images, styles и соответствующие файлы(вфкинуть из html все по папкам). Все картинки должны лежать в папке images
    2) скопировать сайт батлн нет в папку статик.
    3) сервер по пути localhost:8082/ должен открывать страницу батллнета с подгрузкой всех статических файлов(пункт 6* из пред. домашки)
    4) реализовать отправку на почту сообщения с логином и паролем введеным на странице battlenet на вашу почту с помощью httpClient. 
*/

internal class Program
{
    private static bool _isRunning = true;

    static async Task Main(string[] args)
    {
        Console.CancelKeyPress += delegate (object? sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            _isRunning = false;
        };

        try
        {
            using (var server = new HttpServer())
            {
                server.Start();

                while (Console.ReadLine() != "stop" && _isRunning) { }
            }
        }
        catch (Exception ex)
        {
           Console.WriteLine(String.Format("Во время работы сервера произошла непредвиденная ошибкаю {0}", ex.Message));
        }
    }
}