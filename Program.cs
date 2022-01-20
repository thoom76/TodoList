using System.Diagnostics;
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
            var parser = Parser.Default;

            // Configure Serilog to work with spectre console.
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Spectre("{Timestamp:HH:mm:ss} [{Level}] {Message:lj}{NewLine}{Exception}")
                .MinimumLevel.Information()
                .CreateLogger();

            try
            {
                var parsed = parser
                    .ParseVerbSets(
                        args,
                        typeof(CreateVerbSet),
                        typeof(UpdateVerbSet),
                        typeof(CompleteVerbSet),
                        typeof(ListVerb));
                
                // Map the verbs.
                parsed.MapResult(
                    (IVerb verb) =>
                    {
                        verb.OnParse();
                        return parsed;
                    },
                    _ => parsed);
            }
            catch (Exception e)
            {
                if (Debugger.IsAttached)
                {
                    Log.Error(e, e.Message);   
                }
                else
                {
                    Log.Error(e.Message);
                }
            }
        }
    }
}
