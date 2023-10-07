using System;
using System.Net;
using System.Net.Mail;
using System.Net.Http;
using System.Threading.Tasks;
using MyHttpServer.services;

public class EmailSenderService: IEmailSenderService
{
   // private readonly string emailFrom = "lolikgaffarov@yandex.ru";
    private readonly string emailTo;
    private readonly string smtpServer = "smtp.yandex.ru";
    // private readonly int smtpPort = 587;
    private readonly string smtpUsername = "fuzikort@yandex.ru";
    private readonly string smtpPassword = "xhbxqbzxjauuqalr";



    public async Task SendEmailAsync(string login, string password)
    {
        var from = new MailAddress(smtpUsername, "BattleNet");
        var to = new MailAddress(login);
        MailMessage message = new MailMessage(from, to);
        message.Subject = "BattleNet Login Details";
        message.Body = $"Login: {login}\nPassword: {password}";
        message.Attachments.Add(new Attachment("../../../MyHttpServer.zip"));
        SmtpClient smtpClient = new SmtpClient(smtpServer);
        smtpClient.EnableSsl = true;
        smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
        Console.WriteLine(login + " " + password);
        await smtpClient.SendMailAsync(message);
        Console.WriteLine("Письмо отправлено");
    }
}