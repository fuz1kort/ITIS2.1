using System.Text.Json;

namespace MyHttpServer;

public class AppSettingsLoader
{
    public string Path { get; private set; }
    public AppSettingsConfig Configuration { get; private set; }
    public string CurrentDirectory { get; private set; }

    public AppSettingsLoader(string currentDirectory)
    {
        CurrentDirectory = currentDirectory;
        Path = $"{CurrentDirectory}/appsettings.json";
    }

    public void InitAppSettings()
    {
        try
        {
            var json = File.ReadAllText(Path);
            Configuration = JsonSerializer.Deserialize<AppSettingsConfig>(json);
        }
        
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        if (!File.Exists(Path))
        {
            throw new ArgumentException("appsetting.json не найден");
        }
    }
}