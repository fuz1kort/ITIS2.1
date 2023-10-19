using Newtonsoft.Json;

namespace MyHttpServer.Configuration;

public class AppSettingsConfig
{
    [JsonProperty]
    public ushort Port { get; private set; }
    
    [JsonProperty]
    public string Address { get; private set; }
    
    [JsonProperty]
    public string StaticFilesPath { get; private set; }
    
    [JsonProperty]
    public string SmtpUsername { get; private set; }
    
    [JsonProperty]
    public string SmtpPassword { get; private set; }
    
    [JsonProperty]
    public string SmtpServer { get; private set; }
    
    [JsonProperty]
    public ushort SmtpPort { get; private set; }
    
    
    public AppSettingsConfig(ushort port = 0, string address = "", string staticFilesPath = "",
                            string smtpUsername = "", string smtpPassword = "", string smtpServer = "", ushort smtpPort = 0)
    {
        Port = port;
        Address = address;
        StaticFilesPath = staticFilesPath;
        SmtpUsername = smtpUsername;
        SmtpPassword = smtpPassword;
        SmtpServer = smtpServer;
        SmtpPort = smtpPort;
    }

    
}