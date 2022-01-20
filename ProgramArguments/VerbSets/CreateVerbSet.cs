using CommandLine;
using Serilog;
using TodoList.Extensions;
using TodoList.Models;

namespace TodoList.ProgramArguments.VerbSets;

[Verb("create", HelpText = "Create commands.")]
public class CreateVerbSet : IVerbSet
{
    [Option('v', "verbose", Required = false, HelpText = "Give verbose feedback.")]
    public bool Verbose { get; set; } = false;
    
    /// <summary>
    /// The sub-verb used to create a category.
    /// </summary>
    [Verb("category", HelpText = "Create a new category.")]
    public class Category : IVerb
    {
        [Option('n', "name", Required = true, HelpText = "The name of the category.")]
        public string Name { get; set; } = string.Empty;
                
        [Option('d', "description", Required = false, HelpText = "The Description of the category.")]
        public string? Description { get; set; } = null;
        
        [Option('o', "objectives", Required = false, Separator = ',', HelpText = "A list of objective names to add to the newly created category.")]
        public IList<ObjectiveModel> Objectives { get; set; } = new List<ObjectiveModel>();

        public void OnParse()
        {
            if (Storage.GetModelByName<CategoryModel>(Name) is not null)
            {
                throw new Exception($"Can not add a '{nameof(Category)}' with the same {nameof(Name)} '{Name}'!");
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

    /// <summary>
    /// The sub-verb used to create a objective.
    /// </summary>
    [Verb("objective", HelpText = "Create a new objective.")]
    public class Objective : IVerb
    {
        [Option('n', "name", Required = true, HelpText = "The name of the objective.")]
        public string Name { get; set; } = string.Empty;
                
        [Option('d', "description", Required = false, HelpText = "The Description of the objective.")]
        public string? Description { get; set; } = null;

        public void OnParse()
        {
            throw new NotImplementedException();
        }
    }

    /// <inheritdoc/>
    public ParserResult<object> OnParse(Parser parser, IEnumerable<string> argsToParse) => 
        parser.ParseVerbs(argsToParse, typeof(Category), typeof(Objective));
}