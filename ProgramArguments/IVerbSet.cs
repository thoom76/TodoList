using CommandLine;

namespace TodoList.ProgramArguments;

public interface IVerb
{
    /// <summary>
    /// The method that is called when the set must be parsed.
    /// </summary>
    public void OnParse();
}

public interface IVerbSet
{
    /// <summary>
    /// The method that is called when the set must be parsed.
    /// </summary>
    /// <param name="parser"></param>
    /// <param name="argsToParse"></param>
    /// <returns><see cref="ParserResult{T}"/></returns>
    public ParserResult<object> OnParse(Parser parser, IEnumerable<string> argsToParse);
}