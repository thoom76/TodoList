using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using CommandLine;
using Spectre.Console;
using TodoList.Models;
using TodoList.ProgramArguments;

namespace TodoList
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            _ = Parser.Default.ParseSetArguments<CreationVerbSet, ListVerb>(args, OnVerbSetParsed);
        }

        private static ParserResult<object> OnVerbSetParsed(Parser parser,
            Parsed<object> parsed,
            IEnumerable<string> argsToParse,
            bool containedHelpOrVersion)
        {
            return parsed.MapResult(
                (IVerbSet verbSet) => verbSet.OnParse(parser, argsToParse),
                (IVerb verb) =>
                {
                    verb.OnParse();
                    return parsed;
                },
                (_) => parsed);
        }
    }
}
