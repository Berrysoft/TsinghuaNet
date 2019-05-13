using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace TsinghuaNet.CrossPlatform.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "关于";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://github.com/Berrysoft/TsinghuaNet")));
        }

        public ICommand OpenWebCommand { get; }
    }
}