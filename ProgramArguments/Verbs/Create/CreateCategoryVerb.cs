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
    public List<ObjectiveModel> Objectives { get; set; } = new List<ObjectiveModel>();

    public void OnParse()
    {
        if (Storage.GetModelByName<CategoryModel>(Name) is not null)
        {
            throw new Exception($"Can not add a category with the same {nameof(Name)} '{Name}'!");
        }

        var modelGuid = Storage.InsertModel(new CategoryModel
        {
            Name = Name,
            Description = Description,
            Objectives = Objectives
        });
        
        Log.Information("{category} with ID {id} created.", nameof(CategoryModel), modelGuid);
    }
}