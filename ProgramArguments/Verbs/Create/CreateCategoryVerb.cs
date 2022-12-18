using System.Collections.ObjectModel;
using CommandLine;
using Serilog;
using TodoList.Models;

namespace TodoList.ProgramArguments.Verbs.Create;


/// <summary>
/// The sub-verb used to create a category.
/// </summary>
[Verb("category", HelpText = "Create a new category.")]
public class CreateCategoryVerb : IVerb
{
    [Option('n', "name", Required = true, HelpText = "The name of the category.")]
    public string Name { get; set; } = string.Empty;
                
    [Option('d', "description", Required = false, HelpText = "The Description of the category.")]
    public string? Description { get; set; } = null;

    [Option('o', "objectives", Required = false, Separator = ',', HelpText = "A list of objective names to add to the newly created category.")]
    public IList<string> Objectives { get; set; } = new List<string>();

    public void OnParse()
    {
        if (Storage.GetModelByName<CategoryModel>(Name) is not null)
        {
            throw new Exception($"Can not add a category with the same {nameof(Name)} '{Name}'!");
        }

        var newCategory = new CategoryModel
        {
            Name = Name,
            Description = Description
        };

        var existingCategoryNames = new HashSet<string>();
        foreach (var objectiveName in Objectives)
        {
            if (!existingCategoryNames.Add(objectiveName))
            {
                throw new Exception($"Can not add an objective with the same description '{objectiveName}'!");
            }
            
            newCategory.Objectives.Add(new ObjectiveModel
            {
                Name = objectiveName,
                Description = Description
            });
        }

        var modelGuid = Storage.InsertModel(newCategory);
        
        Log.Information("{category} with ID {id} created.", nameof(CategoryModel), modelGuid);
    }
}