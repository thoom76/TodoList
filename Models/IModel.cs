namespace TodoList.Models;

public interface IModel
{
    public Guid Guid { get; init; }
    public string Name { get; set; }
    public string? Description { get; set; }
}