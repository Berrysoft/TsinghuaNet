using System;
using System.Windows.Input;

namespace TsinghuaNet.Uno.ViewModels
{
    class ChangeEnumCommand<T> : ICommand
        where T : struct, Enum
    {
        private Action<T> action;

        public ChangeEnumCommand(Action<T> action) => this.action = action;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => action(Enum.Parse<T>(parameter.ToString()));
    }
}
