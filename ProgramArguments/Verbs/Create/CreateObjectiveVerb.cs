using CommandLine;
using Serilog;
using TodoList.Models;

namespace TodoList.ProgramArguments.Verbs.Create;

/// <summary>
/// The sub-verb used to create a objective.
/// </summary>
[Verb("objective", HelpText = "Create a new objective.")]
public class CreateObjectiveVerb : IVerb
{
    [Option('n', "name", Required = true, HelpText = "The name of the objective.")]
    public string Name { get; set; } = string.Empty;
        
    [Option('c', "category", Required = true, HelpText = "The category where the objective belongs to.")]
    public string Category { get; set; } = string.Empty;
                
    [Option('d', "description", Required = false, HelpText = "The Description of the objective.")]
    public string? Description { get; set; } = null;

    public void OnParse()
    {
        var category = Storage.GetModelByName<CategoryModel>(Category);
        if (category is null)
        {
            throw new Exception($"Can not find a category with name '{Category}'");
        }

        if (category.Objectives.Any(o => o.Name == Name))
        {
            throw new Exception($"Can not add an objective with the same {nameof(Name)} '{Name}'!");         
        }
            
        category.Objectives.Add(new ObjectiveModel
        {
            Name = Name,
            Description = Description
        });

        if (!Storage.UpdateModel(category))
        {
            throw new Exception($"Failed to insert an objective to category '{Category}'");
        };
        
        Log.Information("objective {objectiveName} added to {category}", Name, Category);
    }
}