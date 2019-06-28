using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TsinghuaNet.Uno.ViewModels
{
    class NavigateBackCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

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
