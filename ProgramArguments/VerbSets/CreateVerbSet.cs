using CommandLine;
using TodoList.Extensions;
using TodoList.ProgramArguments.Verbs.Create;

namespace TodoList.ProgramArguments.VerbSets;

[Verb("create", HelpText = "Create commands.")]
public class CreateVerbSet : IVerbSet
{
    /// <inheritdoc/>
    public ParserResult<object> OnParse(Parser parser, IEnumerable<string> argsToParse) => 
        parser.ParseVerbSets(argsToParse, typeof(CreateCategoryVerb), typeof(CreateObjectiveVerb));
}