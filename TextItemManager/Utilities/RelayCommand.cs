using System;
using System.Threading.Tasks;
using System.Windows.Input;

public class RelayCommand : ICommand
{
    private readonly Func<Task> _asyncExecute;
    private readonly Action _syncExecute;
    private readonly Func<bool> _canExecute;

    public RelayCommand(Func<Task> asyncExecute, Func<bool> canExecute = null)
    {
        _asyncExecute = asyncExecute ?? throw new ArgumentNullException(nameof(asyncExecute));
        _canExecute = canExecute;
    }

    public RelayCommand(Action syncExecute, Func<bool> canExecute = null)
    {
        _syncExecute = syncExecute ?? throw new ArgumentNullException(nameof(syncExecute));
        _canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

    public async void Execute(object parameter) => await (_asyncExecute != null ? ExecuteAsync() : Task.Run(ExecuteSync));

    private async Task ExecuteAsync() => await _asyncExecute();

    private void ExecuteSync() => _syncExecute?.Invoke();
}
