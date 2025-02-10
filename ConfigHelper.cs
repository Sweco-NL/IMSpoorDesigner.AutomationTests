using System.IO;
using System.Text.Json;

namespace IMSpoorDesigner.AutomationTests
{
    public static class ConfigHelper
    {
        public static string GetApplicationPath()
        {
            var configFilePath = "appsettings.json"; // Zorg dat het bestand gekopieerd wordt naar de output directory
            if (!File.Exists(configFilePath))
            {
                throw new FileNotFoundException("Config file not found: " + configFilePath);
            }

            var json = File.ReadAllText(configFilePath);
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("ApplicationPath").GetString();
        }
    }
}