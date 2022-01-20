using LiteDB;
using TodoList.Models;

namespace TodoList;

public class Storage : IDisposable
{
    private static readonly ILiteDatabase _db;

    static Storage()
    {
        var path = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/Todo.db";
        _db = new LiteDatabase(path);
    }
    
    public static IEnumerable<TModel> GetAllModels<TModel>() 
        where TModel : IModel
    {
        var collection = _db.GetCollection<TModel>(typeof(TModel).Name);
        var col = collection.Query()
            .ToEnumerable();

        return col;
    }
    
    public static IEnumerable<TModel> GetModelsByName<TModel>(string name) 
        where TModel : IModel
    {
        var collection = _db.GetCollection<TModel>(typeof(TModel).Name);
        return collection.Query()
            .Where(o => o.Name.Contains(name))
            .ToEnumerable();
    }
    
    public static TModel? GetModelByName<TModel>(string name) 
        where TModel : IModel
    {
        var collection = _db.GetCollection<TModel>(typeof(TModel).Name);
        return collection.Query()
            .Where(o => o.Name == name)
            .SingleOrDefault();
    }

    public static TModel? GetModelById<TModel>(Guid id)
        where TModel : IModel
    {
        var collection = _db.GetCollection<TModel>(typeof(TModel).Name);
        return collection.FindById(id);
    }

    public static Guid InsertModel<TModel>(TModel model)
        where TModel : IModel
    {
        var collection = _db.GetCollection<TModel>(typeof(TModel).Name);
        return collection.Insert(model).AsGuid;
    }

    public static bool UpdateModel<TModel>(TModel model)
        where TModel : IModel
    {
        var collection = _db.GetCollection<TModel>(typeof(TModel).Name);
        return collection.Update(model);
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}