using Newtonsoft.Json;

public class AppSettingsConfig
{
    public string Port { get; private set; }
    public string Address { get; private set; }
    public string StaticPathFiles { get; private set; }
    public string MailSender { get; private set; }
    public string PasswordSender { get; private set; }
    public string ToEmail { get; private set; }
    public string SmtpServerHost { get; private set; }
    public ushort SmtpServerPort { get; private set; }

    public AppSettingsConfig(string port = "",
        string address = "",
        string staticFilesPath = "",
        string mailSender = "",
        string passwordSender = "",
        string toEmail = "",
        string smtpServerHost = "",
        ushort smtpServerPort = 0)
    {
        Port = port;
        Address = address;
        StaticPathFiles = staticFilesPath;
        MailSender = mailSender;
        PasswordSender = passwordSender;
        ToEmail = toEmail;
        SmtpServerHost = smtpServerHost;
        SmtpServerPort = smtpServerPort;
    }
}