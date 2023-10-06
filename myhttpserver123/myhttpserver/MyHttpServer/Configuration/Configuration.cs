using System.Text.Json;

namespace MyHttpServer.Configuration
{
    public static class Configuration
    {
        private const string configName = "appsettings.json";
        public static Appsettings Config { get; private set; }

        static Configuration()
        {
            try
            {
                using (var configFile = new FileStream(configName, FileMode.Open))
                {
                    Config = JsonSerializer.Deserialize<Appsettings>(configFile) ?? throw new Exception("Конфигурационный файл некорректен");
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(String.Format("Файл {0} не найден", configName));
                throw ex;
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("Произошла непредвиденная ошибка во время чтения файла конфигурации {0}", ex.Message));
                throw ex;
            }
        }
    }
}
