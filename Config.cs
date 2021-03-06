using Microsoft.Extensions.Configuration;

namespace TodoList;

public static class Config
{
    private static IConfiguration? _config;

    private static IConfiguration? Configuration {
        get {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            _config = builder.Build();
            return _config;
        }
    }
    
    public static readonly string DatabasePath = Configuration!["AppSettings:DatabasePath"];
}