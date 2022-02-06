using CommandLine;
using Serilog;
using TodoList.Models;

namespace TodoList.ProgramArguments.Verbs.Delete;

/// <summary>
/// The sub-verb used to delete an objective.
/// </summary>
[Verb("category", HelpText = "Delete a certain category.")]
public class DeleteCategoryVerb : IVerb
{
    [Option('c', "category", Required = true, HelpText = "The name of the category.")]
    public string Category { get; set; } = string.Empty;
    
    public void OnParse()
    {
        var category = Storage.GetCategoryByNameOrIndex<CategoryModel>(Category);

        if (!Storage.DeleteModel(category))
        {
            throw new Exception($"Failed to delete category '{Category}'");   
        }

        Log.Information("Category {category} deleted", Category);
    }
}