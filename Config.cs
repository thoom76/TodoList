using System.Configuration;
using Microsoft.Extensions.Configuration;
using Serilog;
using Spectre.Console.Cli;

namespace TodoList;


public static class Config
{
    private static IConfiguration? _config;
    public static IConfiguration? Configuration {
        get {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            _config = builder.Build();
            return _config;
        }
    }
    
    public static string DatabasePath = Configuration!["AppSettings:DatabasePath"];
}