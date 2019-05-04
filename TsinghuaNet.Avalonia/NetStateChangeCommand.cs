using System;
using System.Windows.Input;

namespace TsinghuaNet.Avalonia
{
    class NetStateChangeCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        private MainViewModel model;
        public NetStateChangeCommand(MainViewModel model) => this.model = model;

        public void Execute(object parameter) => model.State = Enum.Parse<NetState>(parameter.ToString());
    }
}
