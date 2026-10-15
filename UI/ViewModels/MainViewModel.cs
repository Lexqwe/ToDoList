using Core.Services;
using System.Collections.ObjectModel;
using UI.Infrastructure;

namespace UI.ViewModels;


public class MainViewModel : ObservableObject
{
    private readonly ITaskService _service;
    private string _newTitle = string.Empty;
    public ObservableCollection<TaskItemViewModel> Tasks { get; } = new();

    public string NewTitle
    {
        get => _newTitle;
        set => SetProperty(ref  _newTitle, value);
    }

    public AsyncRelayCommand AddCommand { get; }

    public MainViewModel(ITaskService service)
    {
        _service = service;
        AddCommand = new AsyncRelayCommand(AddAsync);
    }

    public async Task LoadAsync()
    {
        var items = await _service.GetAllAsync();

        Tasks.Clear();
        foreach (var item in items)
        {
            Tasks.Add(CreateViewModel(item));
        }
    }

    private TaskItemViewModel CreateViewModel(Core.Models.TaskItem model)
    {
        return new TaskItemViewModel(model, OnToggleAsync, OnDeleteAsync);
    }

    private async Task AddAsync()
    {
        if (string.IsNullOrWhiteSpace(NewTitle))
        {
            return;
        }

        await _service.AddAsync(NewTitle, null, null);
        NewTitle = string.Empty;
        await LoadAsync();
    }

    private async Task OnToggleAsync(int id)
    {
        await _service.ToggleAsync(id);
        await LoadAsync();
    }

    private async Task OnDeleteAsync(int id)
    {
        await _service.DeleteAsync(id);
        await LoadAsync();
    }
}
