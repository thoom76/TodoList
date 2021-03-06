using CommandLine;
using TodoList.ProgramArguments;

namespace TodoList.Extensions;

public static class VerbExtension
{
    /// <summary>
    /// Parses a string array of command line arguments.
    /// </summary>
    /// <param name="args">A <see cref="System.String"/> array of command line arguments, normally supplied by application entry point.</param>
    /// <seealso cref="Parser.ParseArguments{T}(System.Collections.Generic.IEnumerable{string})"/>
    /// <returns>A <see cref="CommandLine.ParserResult{Object}"/> containing an instances of <see cref="IVerbSet"/></returns>
    public static ParserResult<object> ParseVerbSets(this Parser parser, IEnumerable<string> args, params Type[] types)
    {
        var verbSets = types.Where(t => t.IsAssignableTo(typeof(IVerbSet)));
        var verbs = types.Where(t => t.IsAssignableTo(typeof(IVerb)));

        // Handle the verb sets.
        var parseArgumentsResult = parser.ParseSetArguments(
            args,
            verbSets,
            verbs,
            delegate(Parser parser, Parsed<object> parsed, IEnumerable<string> argsToParse, bool _)
            {
                return parsed.MapVerbSets(parser, argsToParse);
            });

        // TODO: Make it possible to parse the options within a verb set.

        return parseArgumentsResult;
    }

    public static ParserResult<object> MapVerbSets(this ParserResult<object> parseResult, Parser parser, IEnumerable<string> argsToParse)
    {
        return parseResult.MapResult(
            (IVerbSet verbSet) => verbSet.OnParse(parser, argsToParse),
            _ => parseResult);
    }
}