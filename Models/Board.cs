using LiteDB;

namespace TodoList.Models;

public class Board : IModel
{
    [BsonId] 
    public Guid Guid { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Description { get; set; }
    public IList<Category> Categories { get; set; }
}