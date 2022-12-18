using CommandLine;
using Serilog;
using TodoList.Extensions;
using TodoList.Models;

namespace TodoList.ProgramArguments.Verbs.Complete;

/// <summary>
/// The sub-verb used to create a category.
/// </summary>
[Verb("objective", HelpText = "Complete a certain objective.")]
public class CompleteObjectiveVerb : IVerb
{
    [Option('c', "category", Required = true, HelpText = "The name of the category.")]
    public string Category { get; set; } = string.Empty;
    
    [Option('o', "objectives", Required = true, Separator = ',', HelpText = "The name of the objective.")]
    public IEnumerable<string> Objectives { get; set; } = Array.Empty<string>();

    [Option('u', "unfinished", Required = false, HelpText = "Set the finished state to true or false.")]
    public bool Unfinished { get; set; }
    
    public void OnParse()
    {
        var category = Storage.GetModelByName<CategoryModel>(Category);
        
        // The category might be the index of the category.
        if (category is null && int.TryParse(Category, out var categoryIndex))
        {
            categoryIndex -= 1;
            var categories = Storage.GetAllModels<CategoryModel>().ToList();
            var len = categories.Count;
            if (0 <= categoryIndex && categoryIndex < len)
            {
                category = categories[categoryIndex];
            }
        }
        
        if (category is null)
        {
            throw new Exception($"Can not find a category with name '{Category}'");
        }

        category.SortObjectives();
        
        var objectivesLength = category.Objectives.Count;
        foreach (var objective in Objectives)
        {
            var objectiveModel = category.Objectives.FirstOrDefault(o => o.Name == objective);
            
            // The category might be the index of the category.
            if (objectiveModel is null && int.TryParse(objective, out var objectiveIndex))
            {
                objectiveIndex -= 1;
                if (0 <= objectiveIndex && objectiveIndex < objectivesLength)
                {
                    objectiveModel = category.Objectives[objectiveIndex];
                }
            }
            
            if (objectiveModel is null)
            {
                throw new Exception($"Can not find an objective with name '{objective}'");
            }
            objectiveModel.Completed = !Unfinished;
        }

        if (!Storage.UpdateModel(category))
        {
            throw new Exception($"Failed to insert an objective to category '{Category}'");
        };
        
        Log.Information("Objectives {objectiveName} within {categoryName} are set to {status}", 
            string.Join(',', Objectives), 
            Category, 
            Unfinished ? "Todo" : "Finished"
        );
    }
}