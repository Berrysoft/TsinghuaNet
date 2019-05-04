using System;
using System.Windows.Input;

namespace TsinghuaNet.Avalonia
{
    class NetStateChangeCommand : ICommand
    {
        event EventHandler ICommand.CanExecuteChanged { add { } remove { } }

        bool ICommand.CanExecute(object parameter) => true;

        private readonly MainViewModel model;
        public NetStateChangeCommand(MainViewModel model) => this.model = model;

        public void Execute(object parameter) => model.State = Enum.Parse<NetState>(parameter.ToString());
    }
}
