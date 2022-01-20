using LiteDB;

namespace TodoList.Models;

public class CategoryModel : IModel
{
    [BsonId] 
    public Guid Guid { get; init; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public IList<ObjectiveModel> Objectives { get; set; }

    [BsonCtor]
    public CategoryModel(string name = "")
    {
        Guid = Guid.NewGuid();
        Name = name;
        Description = null;
        Objectives = new List<ObjectiveModel>();
    }
}