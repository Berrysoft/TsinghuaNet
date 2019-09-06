using TsinghuaNet.ViewModels;

namespace TsinghuaNet.XF.ViewModels
{
    class SeparateDetailViewModel : NetViewModelBase
    {
        private static readonly DetailViewModel viewModel = new DetailViewModel();
        public DetailViewModel ViewModel => viewModel;
    }
}
