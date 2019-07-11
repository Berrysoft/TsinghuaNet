using System;
using System.Windows.Input;

namespace TsinghuaNet.Uno.Models
{
    sealed class ChangeEnumCommand<T> : ICommand
        where T : struct, Enum
    {
#pragma warning disable 0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore 0067

        private Action<T> action;

        public ChangeEnumCommand(Action<T> action) => this.action = action;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => action(Enum.Parse<T>(parameter.ToString()));
    }
}
