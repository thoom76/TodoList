using CommandLine;
using TodoList.Extensions;
using TodoList.ProgramArguments.Verbs.Complete;

namespace TodoList.ProgramArguments.VerbSets;

[Verb("complete", HelpText = "Create commands.")]
public class CompleteVerbSet : IVerbSet
{
    /// <inheritdoc/>
    public ParserResult<object> OnParse(Parser parser, IEnumerable<string> argsToParse) =>
        parser.ParseVerbSets(argsToParse, typeof(CompleteObjectiveVerb));
}