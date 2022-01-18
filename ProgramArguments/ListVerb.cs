using System.Text;
using CommandLine;
using Spectre.Console;
using TodoList.Models;

namespace TodoList.ProgramArguments;

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

    public void OnParse()
    {
        var categories = new List<Panel>();
        foreach (var category in Storage.GetAllModels<CategoryModel>())
        {
            var content = new StringBuilder();
            foreach (var objective in category.Objectives)
            {
                content.AppendJoin("\n", objective.Name);
            }

            if (content.Length == 0 )
            {
                content.Append("[green]No tasks![/]");
                content.Append(' ', Math.Max(category.Name.Length - content.Length, 0));
            }

            var panel = new Panel(content.ToString())
                .Header(category.Name)
                .HeaderAlignment(Justify.Center)
                .RoundedBorder();
            
            categories.Add(panel);
        }
        
        AnsiConsole.Write(new Columns(categories)
        {
            Expand = false
        });
    }

    public ParserResult<object> OnParse(Parser parser, IEnumerable<string> argsToParse)
    {
        throw new NotImplementedException();
    }
}