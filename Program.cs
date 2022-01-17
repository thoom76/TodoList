using System.Collections.ObjectModel;
using CommandLine;
using Spectre.Console;
using TodoList.Models;

namespace TodoList
{
    public class Category
    {
        public string CategoryName { get; set; }
        public IList<Objective> Objectives { get; set; }

        public Category(string categoryName)
        {
            CategoryName = categoryName;
            Objectives = new List<Objective>();
        }
    }

    public class Objective
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public Objective(string name, string? description = null)
        {
            Name = name;
            Description = description;
        }
    }

    class Program
    {
        private static IList<Category> _categories = new List<Category>
        {
            new Category("Test category")
        };

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
        
        [Verb("list", HelpText = "List ...")]
        class ListOptions
        {
            
        }

        static void Main(string[] args)
        {
            Parser.Default
                .ParseArguments<BaseOptions, CategoryOptions, ListOptions>(args)
                .WithParsed<BaseOptions>(HandleBaseOptions)
                .WithParsed<CategoryOptions>(HandleCreateOptions)
                .WithParsed<ListOptions>(HandleListOptions)
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

        private static void HandleListOptions(ListOptions obj)
        {
            var storage = new Storage();

            var boards = storage.GetModelByName<Board>("Bier").ToList();

            var categories = new List<Panel>();

            foreach (var category in boards.First().Categories)
            {
                var panel = new Panel("some objective")
                    .Header(category.CategoryName)
                    .RoundedBorder();
                
                categories.Add(panel);
            }
            
            AnsiConsole.Write(new Columns(categories));
        }

        private static void HandleNonParsed(IEnumerable<Error> errors)
        {
            // TODO: Implement.
            // throw new NotImplementedException();
        }
    }
}
