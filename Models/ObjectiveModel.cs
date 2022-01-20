using LiteDB;

namespace TodoList.Models;

public class ObjectiveModel : IModel
{
    [BsonId] 
    public Guid Guid { get; init; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool Completed { get; set; }

    [BsonCtor]
    public ObjectiveModel(string name = "")
    {
        Guid = Guid.NewGuid();
        Name = name;
        Description = null;
    }
}