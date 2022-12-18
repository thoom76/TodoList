using CommandLine;
using Serilog;
using TodoList.Extensions;
using TodoList.Models;

namespace TodoList.ProgramArguments.Verbs.Delete;

/// <summary>
/// The sub-verb used to create a category.
/// </summary>
[Verb("category", HelpText = "Delete a certain category.")]
public class DeleteCategoryVerb : IVerb
{
    [Option('c', "category", Required = true, HelpText = "The name of the category.")]
    public string Category { get; set; } = string.Empty;
    
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

        if (!Storage.DeleteModel<CategoryModel>(category.Guid))
        {
            throw new Exception($"Failed to delete category '{Category}'");
        }

        Log.Information("Category {categoryName} is deleted",
            Category
        );
    }
}