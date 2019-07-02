using System;
using System.ComponentModel;
using System.Windows.Input;

namespace TsinghuaNet.Models
{
    public class Command : ICommand
    {
        private NetViewModelBase viewModel;
        private Action<object> action;
        public Command(NetViewModelBase viewModel, Action action) : this(viewModel, o => action())
        {
        }
        public Command(NetViewModelBase viewModel, Action<object> action)
        {
            this.viewModel = viewModel;
            this.action = action;
            viewModel.PropertyChanged += OnModelPropertyChanged;
        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsBusy")
                OnCanExecuteChanged();
        }

        public event EventHandler CanExecuteChanged;
        protected virtual void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public void Execute(object parameter)
        {
            try
            {
                viewModel.IsBusy = true;
                action?.Invoke(parameter);
            }
            finally
            {
                viewModel.IsBusy = false;
            }
        }

        public bool CanExecute(object parameter) => !viewModel.IsBusy;
    }
}
