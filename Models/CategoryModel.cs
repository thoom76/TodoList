using System.Reflection;
using LiteDB;

namespace TodoList.Models;


public class CategoryModel : IModel, IComparable<CategoryModel>
{
    [BsonId] 
    public Guid Guid { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = null;
    public int Priority { get; set; } = 0;
    public List<ObjectiveModel> Objectives { get; set; } = new();

    public int CompareTo(CategoryModel? other)
    {
        if (other is null)
        {
            return 1;
        }
        
        if (Priority > other.Priority)
        {
            return 1;
        }

        if (Priority < other.Priority)
        {
            return -1;
        }
        
        return string.CompareOrdinal(Name, other.Name);
    }
}