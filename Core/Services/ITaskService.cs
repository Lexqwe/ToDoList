using Core.Models;

namespace Core.Services;

public interface ITaskService
{
    Task<List<TaskItem>> GetAllAsync();
    Task<TaskItem?> GetByIdAsync(int id);
    Task<TaskItem> AddAsync(string title, int? categoryId, int? parentId);
    Task DeleteAsync(int id);
    Task ToggleAsync(int id);
    Task UpdateAsync(int id, string title, int? categoryId);

    Task<List<Category>> GetCategoriesAsync();
    Task<Category> AddCategoryAsync(string name, string color);
    Task DeleteCategoryAsync(int id);
}
