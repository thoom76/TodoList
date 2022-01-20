using CommandLine;
using Serilog;
using TodoList.Models;

namespace TodoList.ProgramArguments.Verbs.Complete;

/// <summary>
/// The sub-verb used to create a category.
/// </summary>
[Verb("objective", HelpText = "Complete a certain objective.")]
public class CompleteObjective : IVerb
{
    [Option('c', "category", Required = true, HelpText = "The name of the category.")]
    public string Category { get; set; } = string.Empty;
    
    [Option('o', "objectives", Required = true, Separator = ',', HelpText = "The name of the objective.")]
    public IList<string> Objectives { get; set; } = new List<string>();
    
    [Option('f', "finished", Required = false, HelpText = "Set the finished state to true or false.")]
    public bool Finished { get; set; } = true;
    
    public void OnParse()
    {
        var category = Storage.GetModelByName<CategoryModel>(Category);
        if (category is null)
        {
            throw new Exception($"Can not find a category with name '{Category}'");
        }

        foreach (var objectiveName in Objectives)
        {
            var objective = category.Objectives.FirstOrDefault(o => o.Name == objectiveName);
            if (objective is null)
            {
                throw new Exception($"Can not find an objective with name '{objectiveName}'");
            }
            objective.Completed = Finished;
        }

        if (!Storage.UpdateModel(category))
        {
            throw new Exception($"Failed to insert an objective to category '{Category}'");
        };
        
        Log.Information("Objectives {objectiveName} within {categoryName} are set to {status}", 
            string.Join(',', Objectives), 
            Category, 
            Finished ? "Finished" : "Todo"
        );
    }
}