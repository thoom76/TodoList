using LiteDB;

namespace TodoList.Models;

public interface IModel
{
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}