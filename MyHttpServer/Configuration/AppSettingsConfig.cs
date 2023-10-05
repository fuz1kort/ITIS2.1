using System.Reflection;
using Newtonsoft.Json;

namespace MyHttpServer.Configuration;

public class AppSettingsConfig
{
    [JsonProperty]
    public string Port { get; private set; }
    [JsonProperty]
    public string Address { get; private set; }
    [JsonProperty]
    public string StaticFilesPath { get; private set; }
    
    public static AppSettingsConfig LoadServerConfig()
    {
        try
        {
            string appSettingsPath = "./appsettings.json";
            string json = File.ReadAllText(appSettingsPath);
            var config = JsonConvert.DeserializeObject<AppSettingsConfig>(json);
            EnsureStaticFilesPath(config);
            return config;
        }
        
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке конфигурации из файла appsettings.json: {ex.Message}");
            return null;
        }
    }
    
    private static void EnsureStaticFilesPath(AppSettingsConfig config)
    {
        string projectDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string staticFolderPath = Path.Combine(projectDirectory, config.StaticFilesPath);

        if (!Directory.Exists(staticFolderPath))
        {
            try
            {
                Directory.CreateDirectory(staticFolderPath);
                Console.WriteLine($"Создана папка для статических файлов: {staticFolderPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании папки для статических файлов: {ex.Message}");
            }
        }
    }
}