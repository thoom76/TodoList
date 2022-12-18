using LiteDB;
using TodoList.Models;

namespace TodoList;

public class Storage : IDisposable
{
    private static readonly ILiteDatabase _db;

    static Storage()
    {
        _db = new LiteDatabase(Config.DatabasePath);
    }
    
    public static IEnumerable<TModel> GetAllModels<TModel>() 
        where TModel : IModel
    {
        var collection = GetCollection<TModel>();
        var col = collection.Query()
            .ToEnumerable();

        return col;
    }
    
    public static IEnumerable<TModel> GetModelsByName<TModel>(string name) 
        where TModel : IModel
    {
        var collection = GetCollection<TModel>();
        return collection.Query()
            .Where(o => o.Name.Contains(name))
            .ToEnumerable();
    }
    
    public static TModel? GetModelByName<TModel>(string name) 
        where TModel : IModel
    {
        var collection = GetCollection<TModel>();
        return collection.Query()
            .Where(o => o.Name == name)
            .SingleOrDefault();
    }

    public static TModel? GetModelById<TModel>(Guid id)
        where TModel : IModel
    {
        var collection = GetCollection<TModel>();
        return collection.FindById(id);
    }

    public static Guid InsertModel<TModel>(TModel model)
        where TModel : IModel
    {
        var collection = GetCollection<TModel>();
        return collection.Insert(model).AsGuid;
    }

    public static bool UpdateModel<TModel>(TModel model)
        where TModel : IModel
    {
        var collection = GetCollection<TModel>();
        return collection.Update(model);
    }

    public static bool DeleteModel<TModel>(Guid id)
    {
        var collection = GetCollection<TModel>();
        return collection.Delete(id);
    }

    private static ILiteCollection<TModel> GetCollection<TModel>()
    {
        return _db.GetCollection<TModel>(typeof(TModel).Name);
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}