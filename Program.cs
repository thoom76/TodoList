using CommandLine;
using Serilog;
using Serilog.Sinks.Spectre;
using TodoList.Extensions;
using TodoList.ProgramArguments;
using TodoList.ProgramArguments.Verbs;
using TodoList.ProgramArguments.VerbSets;

namespace TodoList
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            // Configure Serilog to work with spectre console.
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Spectre("{Timestamp:HH:mm:ss} [{Level}] {Message:lj}{NewLine}{Exception}")
                .MinimumLevel.Information()
                .CreateLogger();

            try
            {
                _ = Parser.Default.ParseVerbs(args, 
                    typeof(CreateVerbSet),
                    typeof(UpdateVerbSet),
                    typeof(CompleteVerbSet),
                    typeof(ListVerb));
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
        }
    }
}
