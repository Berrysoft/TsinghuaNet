using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TsinghuaNet.Avalonia
{
    abstract class NetCommand : ICommand
    {
        protected MainViewModel model;
        protected bool executing;
        public NetCommand(MainViewModel model)
        {
            this.model = model;
            model.PropertyChanged += OnModelPropertyChanged;
        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "State")
            {
                OnCanExecuteChanged(EventArgs.Empty);
            }
        }

        public event EventHandler CanExecuteChanged;
        protected virtual void OnCanExecuteChanged(EventArgs e) => CanExecuteChanged?.Invoke(this, e);

        public bool CanExecute(object parameter)
        {
            return !executing && model.State > NetState.Unknown && model.State <= NetState.Auth6;
        }

        public async void Execute(object parameter)
        {
            executing = true;
            OnCanExecuteChanged(EventArgs.Empty);
            try
            {
                var helper = model.GetHelper();
                await ExecuteAsync(helper);
            }
            finally
            {
                executing = false;
                OnCanExecuteChanged(EventArgs.Empty);
            }
        }

        protected abstract Task ExecuteAsync(IConnect helper);
    }

    class LoginCommand : NetCommand
    {
        public LoginCommand(MainViewModel model) : base(model) { }

        protected override async Task ExecuteAsync(IConnect helper)
        {
            if (helper != null)
                await helper.LoginAsync();
            await model.RefreshAsync(helper);
        }
    }

    class LogoutCommand : NetCommand
    {
        public LogoutCommand(MainViewModel model) : base(model) { }

        protected override async Task ExecuteAsync(IConnect helper)
        {
            if (helper != null)
                await helper.LogoutAsync();
            await model.RefreshAsync(helper);
        }
    }

    class RefreshCommand : NetCommand
    {
        public RefreshCommand(MainViewModel model) : base(model) { }

        protected override async Task ExecuteAsync(IConnect helper)
        {
            await model.RefreshAsync(helper);
        }
    }
}
