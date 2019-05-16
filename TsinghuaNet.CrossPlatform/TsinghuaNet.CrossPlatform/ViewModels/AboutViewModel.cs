using System;
using System.Windows.Input;
using TsinghuaNet.Helper;

namespace TsinghuaNet.CrossPlatform.ViewModels
{
    public class AboutViewModel : NetObservableBase
    {
        public AboutViewModel()
        {
            OpenWebCommand = new Command(this, () => Xamarin.Forms.Device.OpenUri(new Uri("https://github.com/Berrysoft/TsinghuaNet")));
        }

        public ICommand OpenWebCommand { get; }
    }
}