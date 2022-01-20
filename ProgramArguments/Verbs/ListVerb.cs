using System.Text;
using CommandLine;
using Spectre.Console;
using TodoList.Models;

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
        var categories = new List<Panel>();
        foreach (var category in Storage.GetAllModels<CategoryModel>())
        {
            var maxRowLength = 0;
            var content = new StringBuilder();

            var objectives = category.Objectives.Select(o =>
            {
                maxRowLength = Math.Max(maxRowLength, o.Name.Length);
                return $"[orange1]{o.Name}[/]";
            });
            
            // Create the content layout string within a category.
            content.AppendJoin('\n', objectives);

            if (content.Length == 0 )
            {
                content.Append("[green]No tasks![/]");
            }
            
            // Append as much spaces to the last row in order for the category title to be visible.
            content.Append(' ', Math.Max(category.Name.Length - maxRowLength, 0));

            var panel = new Panel(content.ToString())
                .Header(category.Name)
                .HeaderAlignment(Justify.Center)
                .RoundedBorder();
            
            categories.Add(panel);
        }

        AnsiConsole.Write(new Columns(categories)
        {
            Expand = Expand
        });
    }
}