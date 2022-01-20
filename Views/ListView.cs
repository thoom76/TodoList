using System.Text;
using Spectre.Console;
using TodoList.Models;

namespace TodoList.Views;

public static class ListView
{
    /// <summary>
    /// List the categories as well as the objectives.
    /// </summary>
    /// <param name="expand">Use the full width of the terminal console.</param>
    public static void ShowTotalView(bool expand = false)
    {
        var categories = new List<Panel>();
        foreach (var category in Storage.GetAllModels<CategoryModel>())
        {
            var maxRowLength = 0;
            var content = new StringBuilder();

            var objectives = category.Objectives.Select(o =>
            {
                maxRowLength = Math.Max(maxRowLength, o.Name.Length);
                return o switch
                {
                    {Completed: true} => $"[green]{Emoji.Known.CheckMark}[/] [grey]{o.Name}[/]",
                    _ => $"{Emoji.Known.EightPointedStar} [orange1]{o.Name}[/]"
                };
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
            Expand = expand
        });
    }
}