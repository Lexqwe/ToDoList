using Core.Data;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class TaskService : ITaskService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public TaskService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<List<TaskItem>> GetAllAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Tasks.ToListAsync();
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Categories.ToListAsync();
    }


    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Tasks.FindAsync(id);
    }


    public async Task<TaskItem> AddAsync(string title, int? categoryId, int? parentId)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be empty", nameof(title));
        }

        await using var db = await _factory.CreateDbContextAsync();

        if (parentId is not null && !await db.Tasks.AnyAsync(t => t.Id == parentId))
        {
            throw new ArgumentException($"Parent {parentId} not found", nameof(parentId));
        }
           
        if (categoryId is not null && !await db.Categories.AnyAsync(c => c.Id == categoryId))
        {
            throw new ArgumentException($"Category {categoryId} not found", nameof(categoryId));
        }

        var task = new TaskItem
        {
            Title = title,
            CategoryId = categoryId,
            ParentId = parentId,
            CreatedAt = DateTime.UtcNow
        };

        db.Tasks.Add(task);
        await db.SaveChangesAsync();

        return task;
    }

    public async Task<Category> AddCategoryAsync(string name, string color)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty", nameof(name));
        }
        if (string.IsNullOrWhiteSpace(color))
        {
            throw new ArgumentException("Color cannot be empty", nameof(color));
        }
        await using var db = await _factory.CreateDbContextAsync();

        var category = new Category
        {
            Name = name,
            Color = color
        };

        db.Categories.Add(category);
        await db.SaveChangesAsync();

        return category;
    }

    public async Task DeleteAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var task = await db.Tasks.FindAsync(id);
        if (task is null)
        {
            throw new ArgumentException($"Task {id} not found", nameof(id));
        }
        db.Tasks.Remove(task);
        await db.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var category = await db.Categories.FindAsync(id);
        if (category is null)
        {
            throw new ArgumentException($"Category {id} not found", nameof(id));
        }
        db.Categories.Remove(category);
        await db.SaveChangesAsync();
    }

    public async Task ToggleAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var task = await db.Tasks.FindAsync(id);
        if (task is null)
        {
            throw new ArgumentException($"Task {id} not found", nameof(id));
        }
        task.IsCompleted = !task.IsCompleted;
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, string title, int? categoryId)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be empty", nameof(title));
        }

        await using var db = await _factory.CreateDbContextAsync();
        var task = await db.Tasks.FindAsync(id);
        if (task is null)
        {
            throw new ArgumentException($"Task {id} not found", nameof(id));
        }
        if (categoryId is not null && !await db.Categories.AnyAsync(c => c.Id == categoryId))
        {
            throw new ArgumentException($"Category {categoryId} not found", nameof(categoryId));
        }

        task.Title = title;
        task.CategoryId = categoryId;
        await db.SaveChangesAsync();
    }
}
