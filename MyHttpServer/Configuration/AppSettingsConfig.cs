using Newtonsoft.Json;

namespace MyHttpServer;

public class AppSettingsConfig
{
    
    [JsonProperty]
    public uint Port { get; private set; }
    [JsonProperty]
    public string Address { get; private set; }
    [JsonProperty]
    public string StaticFilesPath { get; private set; }
    
    public AppSettingsConfig(uint port = 0, string address = "", string staticFilesPath = "")
    {
        Port = port;
        Address = address;
        StaticFilesPath = staticFilesPath;
    }

    
}