using TsinghuaNet.CrossPlatform.Services;
using Xamarin.Forms;
using MvvmHelpers;

namespace TsinghuaNet.CrossPlatform.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        public NetCredential Credential => DependencyService.Get<NetCredential>() ?? new NetCredential();

        bool isBusy = false;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        string title = string.Empty;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
    }
}
