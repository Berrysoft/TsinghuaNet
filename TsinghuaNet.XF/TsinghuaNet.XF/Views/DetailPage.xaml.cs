using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TsinghuaNet.XF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        public DetailPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (Model.InitialDetails == null || Model.InitialDetails.Count == 0)
                Model.RefreshDetails();
            base.OnAppearing();
        }
    }
}