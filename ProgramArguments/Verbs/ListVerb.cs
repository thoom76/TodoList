using CommandLine;
using TodoList.Views;

namespace TodoList.ProgramArguments.Verbs;

[Verb("ls", HelpText = "List the Todos.")]
public class ListVerb : IVerb
{
    [Flags]
    public enum ListFlags
    {
        All,
    }
    
    [Option('o', "options", Required = false, HelpText = "Chose what to list.")]
    public ListFlags WhatToList { get; set; } = ListFlags.All;
    
    [Option('e', "expand", Required = false, HelpText = "Use the full width of the terminal console.")]
    public bool Expand { get; set; } = false;

    public void OnParse()
    {
        ListView.ShowTotalView(Expand);
    }
}