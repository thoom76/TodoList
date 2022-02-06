using System.Collections.Immutable;
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
    [Option('o', "objectives", Required = true, HelpText = "The brief description(s) of the objective(s) to create.")]
    public IList<string> Objectives { get; set; } = new List<string>();
        
    [Option('c', "category", Required = true, HelpText = "The category of the objective(s). Can be both the index as well as the name of the category")]
    public string Category { get; set; } = string.Empty;
                
    [Option('d', "description", Required = false, HelpText = "The Description of the objective.")]
    public string? Description { get; set; } = null;

    public void OnParse()
    {
        var category = Storage.GetCategoryByNameOrIndex<CategoryModel>(Category);

        var existingCategoryNames = category.Objectives.Select(model => model.Name).ToHashSet(); 
        foreach (var objectiveName in Objectives)
        {
            if (!existingCategoryNames.Add(objectiveName))
            {
                throw new Exception($"Can not add an objective with the same description '{objectiveName}'!");
            }
            
            category.Objectives.Add(new ObjectiveModel
            {
                Name = objectiveName,
                Description = Description
            });
        }

        if (!Storage.UpdateModel(category))
        {
            throw new Exception($"Failed to insert an objective to category '{Category}'");
        };
        
        Log.Information("objectives {objectiveNames} added to {category}", string.Join(';', Objectives), Category);
    }
}