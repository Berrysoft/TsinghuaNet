using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TsinghuaNet.Helpers;
using TsinghuaNet.ViewModels;

namespace TsinghuaNet.Models
{
    public class NetCommand : ICommand
    {
        private readonly MainViewModelBase model;
        private Func<IConnect, Task<LogResponse>> executor;

        public NetCommand(MainViewModelBase model, Func<IConnect, Task<LogResponse>> executor)
        {
            this.model = model;
            this.executor = executor;
            model.PropertyChanged += OnModelPropertyChanged;
            model.Credential.PropertyChanged += OnModelPropertyChanged;
        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "State" || e.PropertyName == "IsBusy")
                OnCanExecuteChanged();
        }

        public event EventHandler CanExecuteChanged;
        protected virtual void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute(object parameter) => !model.IsBusy && model.Credential.State != NetState.Unknown;

        public async void Execute(object parameter) => await model.NetCommandExecuteAsync(executor);
    }
}
