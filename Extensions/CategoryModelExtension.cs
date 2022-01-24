using TodoList.Models;

namespace TodoList.Extensions;

public static class CategoryModelExtension
{
    public static void SortObjectives(this CategoryModel category)
    {
        category.Objectives.Sort((a, b) =>
        {
            if (a.Completed == b.Completed)
            {
                return string.CompareOrdinal(a.Name, b.Name);
            }

            // Place completed objectives underneath the open tasks.
            return a.Completed ? 1 : -1;
        });
    }
}