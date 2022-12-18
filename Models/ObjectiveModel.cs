using LiteDB;

namespace TodoList.Models;

public class ObjectiveModel : IModel, IComparable<ObjectiveModel>
{
    [BsonId] 
    public Guid Guid { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = null;
    public int Priority { get; set; } = 0;
    public bool Completed { get; set; } = false;

    public int CompareTo(ObjectiveModel? other)
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
        
        // Place completed objectives underneath the open tasks.
        if (Completed != other.Completed)
        {
            return Completed ? 1 : -1;
        }
        
        return string.CompareOrdinal(Name, other.Name);
    }
}