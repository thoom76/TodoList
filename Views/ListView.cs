using System.Text;
using Spectre.Console;
using TodoList.Extensions;
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
        var panels = new List<Panel>();

        var categories = Storage
            .GetAllModels<CategoryModel>()
            .SortCategories();

        if (categories is null)
        {
            AnsiConsole.Write("[orange]No todo's created (yet)[/]");
            return;
        }

        var categoryIndex = 0;
        foreach (var category in categories)
        {
            categoryIndex++;
            category.SortObjectives();
            
            var categoryTable = new Table()
                .AddColumns("id", "task")
                .Centered()
                .NoBorder()
                .Collapse()
                .HideHeaders();

            var objectivesLength = category.Objectives.Count;
            var maxTaskNameSize = 0;
            var taskCount = 0;
            
            if (objectivesLength == 0)
            {
                const string emptyMessage = "[green]Empty![/]";
                categoryTable.AddRow(emptyMessage);
                maxTaskNameSize = emptyMessage.Length;
            }

            var currentTask = category.Objectives.FirstOrDefault();

            while (currentTask is {Completed: false} && taskCount < objectivesLength)
            {
                maxTaskNameSize = Math.Max(maxTaskNameSize, currentTask.Name.Length);
                categoryTable.AddRow((++taskCount).ToString(), $"{Emoji.Known.EightPointedStar} [orange1]{currentTask.Name}[/]");
                if (taskCount < objectivesLength)
                {
                    currentTask = category.Objectives[taskCount];
                }
            }

            if (taskCount > 0 && objectivesLength - taskCount > 0)
            {
                // Divide line between active and completed tasks.
                categoryTable.AddRow("", $"[grey]{new StringBuilder().Append('—', maxTaskNameSize + 2)}[/]");
            }

            while (currentTask is {Completed: true} && taskCount < objectivesLength)
            {
                maxTaskNameSize = Math.Max(maxTaskNameSize, currentTask.Name.Length);
                categoryTable.AddRow((++taskCount).ToString(), $"[green]{Emoji.Known.CheckMark}[/] [grey]{currentTask.Name}[/]");
                if (taskCount < objectivesLength)
                {
                    currentTask = category.Objectives[taskCount];
                }
            }

            const int leftMargin = 2; // Just the margin spectre.console takes.
            const int rightMargin = 2; // Just the margin spectre.console takes.
            const int columnMargin = 2; // Just the margin spectre.console takes.
            const int nameColumnPrefix = 2; // '✴ ' or '✔ ' before the name column.
            const int categoryPrefix = 3; // 3 = the '<' before and '> ' after the category number.
            
            var width = leftMargin + rightMargin + columnMargin + nameColumnPrefix + category.Objectives.Count.ToString().Length + maxTaskNameSize;
            categoryTable.Width(Math.Max(width, categoryPrefix + leftMargin + rightMargin + category.Name.Length + categoryIndex.ToString().Length));

            panels.Add(new Panel(categoryTable)
                .Header($"<{categoryIndex}> {category.Name}")
                .HeaderAlignment(Justify.Center)
                .Padding(0,0,0,0)
                .RoundedBorder());
        }

        AnsiConsole.Write(new Columns(panels)
        {
            Expand = expand
        });
    }
}