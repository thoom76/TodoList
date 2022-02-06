using CommandLine;
using Serilog;
using TodoList.Extensions;
using TodoList.Models;

namespace TodoList.ProgramArguments.Verbs.Delete;

/// <summary>
/// The sub-verb used to delete an objective.
/// </summary>
[Verb("objective", HelpText = "Delete a certain objective.")]
public class DeleteObjectiveVerb : IVerb
{
    [Option('c', "category", Required = true, HelpText = "The name of the category.")]
    public string Category { get; set; } = string.Empty;
    
    [Option('o', "objectives", Required = true, Separator = ',', HelpText = "The name of the objectives to delete within a certain category.")]
    public IList<string> Objectives { get; set; } = new List<string>();
    
    public void OnParse()
    {
        var category = Storage
            .GetCategoryByNameOrIndex<CategoryModel>(Category)
            .SortObjectives()!;

        foreach (var objective in Objectives)
        {
            var objectiveModel = category.GetObjectiveByNameOrIndex(objective);
            if (!category.Objectives.Remove(objectiveModel))
            {
                throw new Exception($"Failed to remove objective '{objective}' from category '{category.Name}'");
            }
        }
        
        if (!Storage.UpdateModel(category))
        {
            throw new Exception($"Failed to delete an objective to category '{Category}'");
        };

        Log.Information("objectives {objectiveNames} deleted from {category}", string.Join(';', Objectives), Category);
    }
}