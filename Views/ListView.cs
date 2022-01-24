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

        var categoryIndex = 0;
        foreach (var category in Storage.GetAllModels<CategoryModel>())
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

            // 7 = leftMargin (1) + rightMargin (2) + columnMargin(2) + nameColumnPrefix (2).
            var width = 7 + category.Objectives.Count.ToString().Length + maxTaskNameSize;
            // 3 = the '<' before and '> ' after the category number.
            categoryTable.Width(Math.Max(width, category.Name.Length + categoryIndex.ToString().Length + 3));

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