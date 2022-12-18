using CommandLine;
using TodoList.Extensions;
using TodoList.ProgramArguments.Verbs.Delete;

namespace TodoList.ProgramArguments.VerbSets;

[Verb("delete", HelpText = "Delete commands.")]
public class DeleteVerbSet : IVerbSet
{
    /// <inheritdoc/>
    public ParserResult<object> OnParse(Parser parser, IEnumerable<string> argsToParse) => 
        parser.ParseVerbSets(argsToParse, typeof(DeleteCategoryVerb), typeof(DeleteObjectiveVerb));
}