using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TsinghuaNet.Uno.Models
{
    sealed class NavigateBackCommand : ICommand
    {
#pragma warning disable 0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore 0067

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (Window.Current.Content is Frame rootFrame)
            {
                if (rootFrame.CanGoBack)
                {
                    rootFrame.GoBack();
                }
            }
        }
    }
}
