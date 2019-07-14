using System.Threading.Tasks;
using TsinghuaNet.ViewModels;

namespace TsinghuaNet.XF.ViewModels
{
    class MainViewModel : MainViewModelBase
    {
        public override Task LoadSettingsAsync()
        {
            return Task.CompletedTask;
        }

        public override Task SaveSettingsAsync()
        {
            return Task.CompletedTask;
        }
    }
}
