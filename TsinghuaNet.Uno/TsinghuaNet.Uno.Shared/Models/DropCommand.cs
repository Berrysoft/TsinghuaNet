using System;
using System.Net;
using System.Windows.Input;

namespace TsinghuaNet.Uno.Models
{
    sealed class DropCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public event EventHandler<IPAddress> DropUser;

        public void Execute(object parameter) => DropUser?.Invoke(this, (IPAddress)parameter);

        public bool CanExecute(object parameter) => parameter != null;
    }
}
