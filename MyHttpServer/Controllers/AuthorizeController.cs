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
        // var config = AppSettingsLoader.Instance();
        // var response = context.Response;
        // var request = context.Request;
        
        // using (var reader = new StreamReader(request.InputStream))
        // {
        //     var streamRead = reader.ReadToEnd();
        //     var decodedData = HttpUtility.UrlDecode(streamRead, System.Text.Encoding.UTF8);
        //     var values = decodedData.Split("&");
        //     var emailSender = new EmailSenderService(config.Configuration);
        //     var login = values[0].Split("=")[1];
        //     var password = values[1].Split("=")[1];
        //     await emailSender.SendEmailAsync(login, password);
        //     
        //     
        // }
        
        new EmailSenderService(AppSettingsLoader.Instance()?.Configuration).SendEmail(email, password);
        Console.WriteLine("Email was sent successfully!");

        // var buffer = File.ReadAllBytes(config.Configuration!.StaticFilesPath + "/battlenet.html");
        // response.ContentType = "text/html; charset=utf-8";
        // response.ContentLength64 = buffer.Length;
        // await using var output = response.OutputStream;
        //     
        // await output.WriteAsync(buffer);
        // await output.FlushAsync();
    }
}