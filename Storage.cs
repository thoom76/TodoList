using LiteDB;
using TodoList.Extensions;
using TodoList.Models;

namespace TodoList;

public class Storage : IDisposable
{
    private static readonly ILiteDatabase _db;

    static Storage()
    {
        var path = Config.DatabasePath;
        _db = new LiteDatabase(path);
        if (!File.Exists(path))
        {
            File.Create(path);
        }
    }
    
    public static IEnumerable<TModel> GetAllModels<TModel>() 
        where TModel : IModel
    {
        var collection = GetCollection<TModel>();
        return collection.Query().ToEnumerable();
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

    public static TCategoryModel GetCategoryByNameOrIndex<TCategoryModel>(string nameOrIndex)
        where TCategoryModel : CategoryModel
    {
        var model = GetModelByName<TCategoryModel>(nameOrIndex);
        if (model is not null)
        {
            return model;
        }
        
        // The input might be the index of the category.
        if (!int.TryParse(nameOrIndex, out var modelIndex))
        {
            throw new Exception($"Can not find a model with name or index '{nameOrIndex}'");
        }
        
        modelIndex -= 1;
        var models = GetAllModels<TCategoryModel>()
            .SortCategories()!
            .ToList();
        var len = models.Count;

        if (len == 0)
        {
            throw new Exception($"No models found for type '{typeof(TCategoryModel).FullName}'!");
        }
        
        if (0 > modelIndex || modelIndex >= len)
        {
            throw new Exception($"Model index out of range [0,{len-1}] but is actually '{modelIndex}'");
        }
        
        return (TCategoryModel) models[modelIndex];
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

    public static bool DeleteModel<TModel>(TModel model) 
        where TModel : IModel
    {
        var collection = GetCollection<TModel>();
        return collection.Delete(model.Guid);
    }

    private static ILiteCollection<TModel> GetCollection<TModel>() 
        where TModel : IModel
    {
        return _db.GetCollection<TModel>(typeof(TModel).Name);
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}