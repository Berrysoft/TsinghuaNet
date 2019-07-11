using System;
using System.Net;
using System.Windows.Input;

namespace TsinghuaNet.Uno.Models
{
    sealed class DropCommand : ICommand
    {
#pragma warning disable 0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore 0067

        public event EventHandler<IPAddress> DropUser;

        public void Execute(object parameter) => DropUser?.Invoke(this, (IPAddress)parameter);

        public bool CanExecute(object parameter) => parameter != null;
    }
}
