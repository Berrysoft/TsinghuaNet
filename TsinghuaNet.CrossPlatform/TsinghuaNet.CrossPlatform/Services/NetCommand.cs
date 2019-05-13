using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TsinghuaNet.Helper;

namespace TsinghuaNet.CrossPlatform.Services
{
    public class NetCommand : ICommand
    {
        private readonly NetCredential credential;
        private bool executing;
        private Func<IConnect, Task<LogResponse>> executor;

        public NetCommand(NetCredential credential, Func<IConnect, Task<LogResponse>> executor)
        {
            this.credential = credential;
            this.executor = executor;
            credential.PropertyChanged += OnCredentialPropertyChanged;
        }

        private void OnCredentialPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "State")
            {
                OnCanExecuteChanged();
            }
        }

        public event EventHandler CanExecuteChanged;

        protected void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute(object parameter) => !executing && credential.State != NetState.Unknown;

        public async void Execute(object parameter)
        {
            executing = true;
            OnCanExecuteChanged();
            try
            {
                var helper = credential.GetHelper();
                await executor(helper);
            }
            finally
            {
                executing = false;
                OnCanExecuteChanged();
            }
        }
    }
}
