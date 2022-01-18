using LiteDB;
using Microsoft.VisualBasic;
using TodoList.ProgramArguments;

namespace TodoList.Models;

public class ObjectiveModel : IModel
{
    [BsonId] 
    public Guid Guid { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = null;
}