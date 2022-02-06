using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualBasic;
using TodoList.Models;

namespace TodoList.Extensions;

public static class CategoryModelExtension
{
    public static CategoryModel? SortObjectives(this CategoryModel? category)
    {
        if (category is null)
        {
            return null;
        }
        
        category.Objectives.Sort();
        return category;
    }

    public static ObjectiveModel GetObjectiveByNameOrIndex(this CategoryModel category, string objectiveNameOrIndex)
    {
        var objectiveModel = category.Objectives.FirstOrDefault(o => o.Name == objectiveNameOrIndex);
        
        // The category might be the index of the category.
        if (objectiveModel is null && int.TryParse(objectiveNameOrIndex, out var objectiveIndex))
        {
            objectiveIndex -= 1;
            if (0 <= objectiveIndex && objectiveIndex < category.Objectives.Count)
            {
                objectiveModel = category.Objectives[objectiveIndex];
            }
        }
            
        if (objectiveModel is null)
        {
            throw new Exception($"Can not find an objective with name or index: '{objectiveNameOrIndex}'");
        }

        return objectiveModel;
    }

    public static IEnumerable<CategoryModel>? SortCategories(this IEnumerable<CategoryModel>? categories)
    {
        if (categories is null)
        {
            return categories;
        }

        var categoryModels = categories.ToList();
        categoryModels.Sort();
        return categoryModels;
    }
}