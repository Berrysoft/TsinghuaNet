using Syncfusion.XForms.PopupLayout;
using TsinghuaNet.ViewModels;
using Xamarin.Forms.Xaml;

namespace TsinghuaNet.XF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectIPPage : PopupView
    {
        public ConnectIPPage(ConnectionViewModel viewModel)
        {
            InitializeComponent();
            Model.ConnectionModel = viewModel;
        }
    }
}