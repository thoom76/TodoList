using System.ComponentModel;
using CommandLine;

namespace TodoList
{
    class Category
    {
        private readonly string _categoryName;
        private readonly IList<Objective> _objectives;

        public Category(string categoryName)
        {
            _categoryName = categoryName;
            _objectives = new List<Objective>();
        }
    }

    internal class Objective
    {
        private readonly string _name;
        private readonly string _description;

        public Objective(string name, string description)
        {
            _name = name;
            _description = description;
        }
    }

    class Program
    {
        public class BaseOptions
        {
            [Option('f', "force", Required = false, HelpText = "Force a certain command to be executed.")]
            public bool Forced { get; set; }
        }
        
        [Verb("create", HelpText = "...")]
        class CategoryOptions
        {
            [Option('c', "categories", Required = false, HelpText = "...", SetName = "create", Separator = ',')]
            public IEnumerable<string> Categories { get; set; } = new List<string>();
            
            [Option('o', "objectives", Required = false, HelpText = "...", SetName = "create", Separator = ',')]
            public IEnumerable<string> Objectives { get; set; } = new List<string>();
            
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<BaseOptions, CategoryOptions>(args)
                .WithParsed<BaseOptions>(HandleBaseOptions)
                .WithParsed<CategoryOptions>(HandleCreateOptions)
                .WithNotParsed(HandleNonParsed);
        }

        private static void HandleBaseOptions(BaseOptions baseOptions)
        {
            // throw new NotImplementedException();
        }

        private static void HandleCreateOptions(CategoryOptions categoryOptions)
        {
            // throw new NotImplementedException();
        }

        private static void HandleNonParsed(IEnumerable<Error> errors)
        {
            // TODO: Implement.
            // throw new NotImplementedException();
        }
    }
}
