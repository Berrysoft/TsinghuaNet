using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TsinghuaNet.XF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChartPage : ContentPage
    {
        public ChartPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (!Model.IsBusy && (Model.ViewModel.InitialDetails == null || Model.ViewModel.InitialDetails.Count == 0))
                Model.ViewModel.RefreshDetails();
            base.OnAppearing();
        }
    }
}