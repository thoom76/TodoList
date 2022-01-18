using LiteDB;

namespace TodoList.Models;

public class CategoryModel : IModel
{
    [BsonId] 
    public Guid Guid { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = null;
    public IList<ObjectiveModel> Objectives { get; set; } = new List<ObjectiveModel>();
}