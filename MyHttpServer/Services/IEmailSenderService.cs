namespace MyHttpServer.services;

public interface IEmailSenderService
{
    public Task SendEmailAsync(string login, string password);
}