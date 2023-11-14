using System.Net;
using MyHttpServer.Attributes;
using MyHttpServer.Configuration;
using MyHttpServer.Services;

namespace MyHttpServer.Controllers;

[Controller("Authorize")]
public class AuthorizeController
{
    [Post("SendToEmail")]
    public void SendToEmail(string email, string password)
    {
        new EmailSenderService(AppSettingsLoader.Instance()?.Configuration).SendEmail(email, password);
        Console.WriteLine("Email was sent successfully!");
    }
}