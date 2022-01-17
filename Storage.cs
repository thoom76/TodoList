using LiteDB;
using TodoList.Models;

namespace TodoList;

public class Storage : IDisposable
{
    private readonly ILiteDatabase _db;

    public Storage()
    {
        _db = new LiteDatabase("Todo.db");
    }
    
    public IEnumerable<TModel> GetModelByName<TModel>(string name) 
        where TModel : IModel
    {
        var collection = _db.GetCollection<TModel>(typeof(TModel).Name);
        var col = collection.Query()
            .Where(o => o.Name.Contains(name))
            .ToEnumerable();
        return col;
    }

    public TModel? GetModelById<TModel>(Guid id)
        where TModel : IModel
    {
        var collection = _db.GetCollection<TModel>(typeof(TModel).Name);
        return collection.FindById(id);
    }
    
    public void UpdateModel<TModel>(TModel board)
        where TModel : IModel
    {
        var collection = _db.GetCollection<TModel>(typeof(TModel).Name);
        _ = collection.Update(board);
    }

    public void InsertModel<TModel>(TModel board)
        where TModel : IModel
    {
        var collection = _db.GetCollection<TModel>(typeof(TModel).Name);
        _ = collection.Insert(board);
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}