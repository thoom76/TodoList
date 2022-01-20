namespace TodoList.ProgramArguments;

public interface IVerb
{
    /// <summary>
    /// The method that is called when the set must be parsed.
    /// </summary>
    public void OnParse();
}