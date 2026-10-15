using UI.Infrastructure;
using Core.Models;

namespace UI.ViewModels;

public class TaskItemViewModel : ObservableObject
{
    private readonly TaskItem _model;
    public int Id => _model.Id;
    public string Title => _model.Title;
    public bool IsCompleted
    {
        get => _model.IsCompleted;
        set
        {
            if (_model.IsCompleted == value) 
            {
                return;
            }
            _model.IsCompleted = value;
            OnPropertyChanged();
        }
    }

    public AsyncRelayCommand ToggleCommand { get; }
    public AsyncRelayCommand DeleteCommand { get; }

    public TaskItemViewModel(TaskItem model, Func<int, Task> onToggle, Func<int, Task> onDelete)
    {
        _model = model;
        ToggleCommand = new AsyncRelayCommand(() => onToggle(Id));
        DeleteCommand = new AsyncRelayCommand(() => onDelete(Id));
    }
}
