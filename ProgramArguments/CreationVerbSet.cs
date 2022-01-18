using CommandLine;
using TodoList.Models;

namespace TodoList.ProgramArguments;

[Verb("create", HelpText = "Create commands.")]
public class CreationVerbSet : IVerbSet
{
    /// <summary>
    /// The sub-verb used to create a category.
    /// </summary>
    [Verb("category", HelpText = "Create a new category.")]
    public class Category
    {
        [Option('n', "name", Required = true, HelpText = "The name of the category.")]
        public string Name { get; set; } = string.Empty;
                
        [Option('d', "description", Required = false, HelpText = "The Description of the category.")]
        public string? Description { get; set; } = null;
    }

    /// <summary>
    /// The sub-verb used to create a objective.
    /// </summary>
    [Verb("objective", HelpText = "Create a new objective.")]
    public class Objective
    {
        [Option('n', "name", Required = true, HelpText = "The name of the objective.")]
        public string Name { get; set; } = string.Empty;
                
        [Option('d', "description", Required = false, HelpText = "The Description of the objective.")]
        public string? Description { get; set; } = null;
    }

    /// <inheritdoc/>
    public ParserResult<object> OnParse(Parser parser, IEnumerable<string> argsToParse)
    {
        return parser.ParseArguments<Category, Objective>(argsToParse)
            .WithParsed<Category>(OnCreateCategory)
            .WithParsed<Objective>(OnCreatObjective);
    }

    private static void OnCreateCategory(Category category)
    {
        if (Storage.GetModelByName<CategoryModel>(category.Name) is not null)
        {
            throw new Exception($"Can not add a category with the same name '{category.Name}'!");
        }

        var a = Storage.InsertModel(new CategoryModel
        {
            Name = category.Name,
            Description = category.Description,
            // Objectives = ... // TODO: Make it possible to add objectives at creation.
        });
    }

    private static void OnCreatObjective(Objective objective)
    {
        throw new NotImplementedException();
    }
}