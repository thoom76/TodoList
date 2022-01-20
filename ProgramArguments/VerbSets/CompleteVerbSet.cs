using CommandLine;

namespace TodoList.ProgramArguments.VerbSets;

[Verb("complete", HelpText = "Create commands.")]
public class CompleteVerbSet : IVerbSet
{
    public ParserResult<object> OnParse(Parser parser, IEnumerable<string> argsToParse)
    {
        throw new NotImplementedException();
    }
}